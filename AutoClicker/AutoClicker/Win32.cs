using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoClicker
{
    public class Win32
    {
        //Mouse actions
        public const int MouseEventLeftDown = 0x02;
        public const int MouseEventLeftUp = 0x04;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Point pt);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(Point p);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowTitle(IntPtr hWnd)
        {
            var length = GetWindowTextLength(hWnd) + 1;
            var title = new StringBuilder(length);
            GetWindowText(hWnd, title, length);
            return title.ToString();
        }

        public static bool IsMouseInsideRobloxWindow(bool allowEverywhere)
        {
            if (allowEverywhere) return true;

            var w32Mouse = new Point();
            GetCursorPos(ref w32Mouse);
            var hWnd = WindowFromPoint(w32Mouse);

            if (hWnd == IntPtr.Zero) return false;
            
            return GetWindowTitle(hWnd) == "Roblox";
        }
    }
}