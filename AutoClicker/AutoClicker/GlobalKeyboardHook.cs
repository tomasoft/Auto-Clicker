using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoClicker
{
    internal class GlobalKeyboardHook : IDisposable
    {
        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;
        
        // EDT: Added an optional parameter (registeredKeys) that accepts keys to restrict
        // the logging mechanism.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registeredKeys">Keys that should trigger logging. Pass null for full logging.</param>
        public GlobalKeyboardHook(Keys[] registeredKeys = null)
        {
            RegisteredKeys = registeredKeys;
            _windowsHookHandle = IntPtr.Zero;
            _user32LibraryHandle = IntPtr.Zero;
            _hookProc =
                LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            _user32LibraryHandle = LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }



            _windowsHookHandle = SetWindowsHookEx(WhKeyboardLl, _hookProc, _user32LibraryHandle, 0);
            if (_windowsHookHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                if (_windowsHookHandle != IntPtr.Zero)
                {
                    if (!UnhookWindowsHookEx(_windowsHookHandle))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode,
                            $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }

                    _windowsHookHandle = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    _hookProc -= LowLevelKeyboardProc;
                }
            }

            if (_user32LibraryHandle != IntPtr.Zero)
            {
                if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode,
                        $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }

                _user32LibraryHandle = IntPtr.Zero;
            }
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IntPtr _windowsHookHandle;
        private IntPtr _user32LibraryHandle;
        private HookProc _hookProc;

        delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        
        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lPfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lPfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out WindowRect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct LowLevelKeyboardInputEvent
        {
            /// <summary>
            /// A virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public int VirtualCode;

            /// <summary>
            /// A hardware scan code for the key. 
            /// </summary>
            public int HardwareScanCode;

            /// <summary>
            /// The extended-key flag, event-injected Flags, context code, and transition-state flag.
            /// This member is specified as follows. An application can use the following values to test the keystroke Flags.
            /// Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected.
            /// If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not
            /// the event was injected from a process running at lower integrity level.
            /// </summary>
            public int Flags;

            /// <summary>
            /// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
            /// </summary>
            public int TimeStamp;

            /// <summary>
            /// Additional information associated with the message. 
            /// </summary>
            public IntPtr AdditionalInformation;


            public bool IsControlPressed;
            public bool IsShiftPressed;
            public bool IsAltPressed;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public uint Type;
            public InputUnion Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput MouseInput;
            [FieldOffset(0)] public KeyBdInput KeyboardInput;
            [FieldOffset(0)] public HardwareInput HardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyBdInput
        {
            public ushort VirtualKey;
            public ushort ScanCode;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        public const int WhKeyboardLl = 13;
        //const int HC_ACTION = 0;

        private const int InputKeyboard = 1;
        private const uint KeyEventFKeyup = 0x0002;
        private const uint KeyEventFScancode = 0x0008;
        private const uint KeyEventFExtendedkey = 0x0001;
        private const uint MapvkVkToVsc = 0;
        private const int WmKeydown = 0x0100;
        private const int WmKeyup = 0x0101;

        public enum KeyboardState
        {
            KeyDown = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // EDT: Replaced VkSnapshot(int) with RegisteredKeys(Keys[])
        public static Keys[] RegisteredKeys;

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var wParamTyped = wParam.ToInt32();

            if (!Enum.IsDefined(typeof(KeyboardState), wParamTyped))
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            
            var o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
            var p = (LowLevelKeyboardInputEvent) o;
            
            p.IsControlPressed = Control.ModifierKeys.HasFlag(Keys.Control);
            p.IsShiftPressed = Control.ModifierKeys.HasFlag(Keys.Shift);
            p.IsAltPressed = Control.ModifierKeys.HasFlag(Keys.Alt);

            var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState) wParamTyped);

            // EDT: Removed the comparison-logic from the usage-area so the user does not need to mess around with it.
            // Either the incoming key has to be part of RegisteredKeys (see constructor on top) or RegisteredKeys
            // has to be null for the event to get fired.
            var key = (Keys) p.VirtualCode;
            if (RegisteredKeys != null && !RegisteredKeys.Contains(key))
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            
            var handler = KeyboardPressed;
            handler?.Invoke(this, eventArguments);

            var fEatKeyStroke = eventArguments.Handled;

            return fEatKeyStroke ? (IntPtr) 1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public void SendKeys(Keys key, bool keyDown, IntPtr targetWindow = default)
        {
            var virtualKey = (ushort)((int)key & 0xFF);
            var scanCode = (ushort)MapVirtualKey(virtualKey, MapvkVkToVsc);
            var extended = IsExtendedKey(virtualKey);

            SendInputVirtualKey(virtualKey, scanCode, keyDown, extended);

            if (targetWindow != IntPtr.Zero)
                PostKeyMessage(targetWindow, virtualKey, scanCode, keyDown, extended);
        }

        private static bool IsExtendedKey(ushort virtualKey)
        {
            switch (virtualKey)
            {
                case 0x21:
                case 0x22:
                case 0x23:
                case 0x24:
                case 0x25:
                case 0x26:
                case 0x27:
                case 0x28:
                case 0x2D:
                case 0x2E:
                case 0x5B:
                case 0x5C:
                case 0x6F:
                    return true;
                default:
                    return false;
            }
        }

        private static int BuildKeyLParam(ushort scanCode, bool keyUp, bool extended)
        {
            var lParam = 1 | (scanCode << 16);
            if (extended) lParam |= 1 << 24;
            if (keyUp) lParam |= 3 << 30;
            return lParam;
        }

        private static void PostKeyMessage(IntPtr targetWindow, ushort virtualKey, ushort scanCode, bool keyDown, bool extended)
        {
            var message = keyDown ? WmKeydown : WmKeyup;
            var lParam = BuildKeyLParam(scanCode, !keyDown, extended);
            PostMessage(targetWindow, (uint)message, (IntPtr)virtualKey, (IntPtr)lParam);
        }

        private void SendInputVirtualKey(ushort virtualKey, ushort scanCode, bool keyDown, bool extended)
        {
            uint flags = keyDown ? 0 : KeyEventFKeyup;
            if (extended) flags |= KeyEventFExtendedkey;

            var input = new Input
            {
                Type = InputKeyboard,
                Data = new InputUnion
                {
                    KeyboardInput = new KeyBdInput
                    {
                        VirtualKey = virtualKey,
                        ScanCode = scanCode,
                        Flags = flags,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            var inputs = new[] { input };
            var inputSize = Marshal.SizeOf(typeof(Input));

            if (SendInput((uint)inputs.Length, inputs, inputSize) != 0) return;

            SendInputScanCode(virtualKey, scanCode, keyDown, extended);
        }

        private void SendInputScanCode(ushort virtualKey, ushort scanCode, bool keyDown, bool extended)
        {
            uint flags = KeyEventFScancode;
            if (!keyDown) flags |= KeyEventFKeyup;
            if (extended) flags |= KeyEventFExtendedkey;

            var input = new Input
            {
                Type = InputKeyboard,
                Data = new InputUnion
                {
                    KeyboardInput = new KeyBdInput
                    {
                        VirtualKey = 0,
                        ScanCode = scanCode,
                        Flags = flags,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            var inputs = new[] { input };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }
    }
}
