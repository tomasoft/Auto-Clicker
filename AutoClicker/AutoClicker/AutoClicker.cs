using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class AutoClicker : Form
    {
        #region Variables

        private int _minTimeDefault = 25;
        private int _maxTimeDefault = 30;
        private const int DelayTime = 5000;
        private const int PressedFor = 500;
        private const string ButtonToPress = "W";
        
        private bool _clickMouse;
        private bool _pressKey;

        private bool _fastMode = true;
        private bool _keepPressed = false;

        private DateTime _lastRightClickTime = DateTime.MinValue;
        private const double RightClickDelayMinutes = 18;

        private GlobalKeyboardHook _globalKeyboardHook;

        #endregion

        #region Ctor

        public AutoClicker()
        {
            InitializeComponent();
            InitializeWaitTimes();
        }

        #endregion

        #region Form Events

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook(new[] { Keys.F8 });
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }


        private void mnuQuit_Click(object sender, EventArgs e)
        {
            ToggleAutoClicker(false);
            ToggleAutoType(false);
            Application.Exit();
        }

        private void AutoClicker_Load(object sender, EventArgs e)
        {
            EnableSettingFields();
            SetupKeyboardHooks();
            notifyIcon1.Icon = Icon.FromHandle(Properties.Resources.mouse_white.GetHicon());
            fastToolStripMenuItem.PerformClick();
        }

        void ToggleKeepPressed()
        {
            _keepPressed = !_keepPressed;
            chkKeepPressed.Checked = _keepPressed;
            keepPressedToolStripMenuItem.CheckState = _keepPressed ? CheckState.Checked : CheckState.Unchecked;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown) return;

            if (e.KeyboardData.IsControlPressed)
            {
                ToggleKeepPressed();
                //ToggleAutoType(!_pressKey);
            }

            if (e.KeyboardData.VirtualCode.Equals((int)Keys.F8))
            {
                ToggleAutoClicker(!_clickMouse);                    
            }
        }

        private void ToggleAutoType(bool active)
        {
            const MessageBoxButtons errorButtons = MessageBoxButtons.OK;
            const MessageBoxIcon errorIcon = MessageBoxIcon.Error;

            if (ValidTypeData(out var errorType))
            {
                if (active)
                {
                    AutoTypeOnNewThread();
                    chkAutoType.Checked = true;
                    _pressKey = true;
                }
                else
                {
                    chkAutoType.Checked = false;
                    _pressKey = false;
                }
            }
            else
            {
                var errorMessage = string.Empty;
                var caption = string.Empty;

                if (errorType == "NON-INT")
                {
                    errorMessage = "Delay Time needs to be non-decimal numbers!";
                    caption = "Invalid input(s)";
                }

                MessageBox.Show(errorMessage, caption, errorButtons, errorIcon);
            }
        }

        private void btnHide_Click(object sender, EventArgs e) => 
            Hide();

        /// <summary>
        /// Start autoClicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuStart_Click(object sender, EventArgs e) => 
            ToggleAutoClicker(true);

        /// <summary>
        /// Stop autoClicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuStop_Click(object sender, EventArgs e) => 
            ToggleAutoClicker(false);

        private void AutoClicker_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) return;
            Hide();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void chkAutoType_CheckedChanged(object sender, EventArgs e) => 
            ToggleAutoType(chkAutoType.Checked);

        #endregion

        #region Form Methods

        /// <summary>
        ///     Set the min/max wait time and mouse movement clicks.
        ///     Min wait time indicates the minimum amount of time waited before the next click.
        ///     Max wait time indicates the maximum amount of time waited before the next click.
        ///     minClickBeforeMouseMove indicates the minimum number of clicks needed before mouse jumps position.
        ///     maxClicksBeforeMouseMove indicates the maximum number of clicks needed before mouse jumps position.
        /// </summary>
        private void InitializeWaitTimes()
        {
            minWait.Text = $@"{_minTimeDefault}";
            maxWait.Text = $@"{_maxTimeDefault}";
            delayTime.Text = $@"{DelayTime}";
            buttonToPress.Text = $@"{ButtonToPress}";
            pressedFor.Text = $@"{PressedFor}";
        }

        /// <summary>
        ///     This is a gatekeeper method to keep only a single auto-clicker thread running
        /// </summary>
        private void ToggleAutoClicker(bool active)
        {
            const MessageBoxButtons errorButtons = MessageBoxButtons.OK;
            const MessageBoxIcon errorIcon = MessageBoxIcon.Error;

            if (ValidFieldData(out var errorType))
            {
                if (active)
                {
                    notifyIcon1.Icon = _keepPressed 
                        ? Icon.FromHandle(Properties.Resources.mouse_red.GetHicon()) : 
                            _fastMode ? Icon.FromHandle(Properties.Resources.mouse_green.GetHicon())
                            : Icon.FromHandle(Properties.Resources.mouse_orange.GetHicon());
                    AutoClickOnNewThread();
                    _clickMouse = true;
                    DisableSettingFields();
                }
                else
                {
                    notifyIcon1.Icon = Icon.FromHandle(Properties.Resources.mouse_white.GetHicon());
                    _clickMouse = false;
                    _pressKey = false;
                    EnableSettingFields();
                }
            }
            else
            {
                string errorMessage;
                string caption;

                if (errorType == "NON-INT")
                {
                    errorMessage = "Wait Time and Mouse Clicks need to be non-decimal numbers!";
                    caption = "Invalid input(s)";
                }
                else
                {
                    errorMessage = "Max clicks/Max time cannot be less than Min clicks/Min time!";
                    caption = "Max < Min Error";
                }

                MessageBox.Show(errorMessage, caption, errorButtons, errorIcon);
            }
        }

        /// <summary>
        ///     Verifies that minClicksBetweenMovement, maxClicksBetweenMovement, minWait, and maxWait have integer values in them.
        /// </summary>
        /// <returns></returns>
        private bool ValidFieldData(out string typeError)
        {
            typeError = "NONE";

            int minWaitValue;
            int maxWaitValue;

            try
            {
                minWaitValue = int.Parse(minWait.Text);
                maxWaitValue = int.Parse(maxWait.Text);
            }
            catch (Exception)
            {
                typeError = "NON-INT";
                return false;
            }

            if (maxWaitValue >= minWaitValue) return true;

            typeError = "MAX LESS THAN MIN";

            return false;
        }

        /// <summary>
        ///     Verifies that minClicksBetweenMovement, maxClicksBetweenMovement, minWait, and maxWait have integer values in them.
        /// </summary>
        /// <returns></returns>
        private bool ValidTypeData(out string typeError)
        {
            typeError = "NONE";

            try
            {
                _ = int.Parse(delayTime.Text);
                _ = int.Parse(pressedFor.Text);
            }
            catch (Exception)
            {
                typeError = "NON-INT";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Creates a new background thread and runs AutoClick() on that thread.
        /// </summary>
        private void AutoClickOnNewThread()
        {
            var t = new Thread(AutoClick)
            {
                IsBackground = true
            };

            t.Start();
        }

        /// <summary>
        ///     Creates a new background thread and runs AutoClick() on that thread.
        /// </summary>
        private void AutoTypeOnNewThread()
        {
            var t = new Thread(AutoType)
            {
                IsBackground = true
            };

            t.Start();
        }

        private void AutoClick()
        {
            do
            {
                var minWaitTime = int.Parse(minWait.Text);
                var maxWaitTime = int.Parse(maxWait.Text);

                var rnd = new Random();
                var timeBetweenClicks = rnd.Next(minWaitTime, maxWaitTime);
                DoMouseClick();
                Thread.Sleep(timeBetweenClicks);

            } while (_clickMouse);

            ResetMouseClickState();
        }

        private void ResetMouseClickState()
        {
            var cursorPosition = GetCursorPosition();
            Win32.mouse_event(Win32.MouseEventLeftDown | Win32.MouseEventLeftUp, cursorPosition.x, cursorPosition.y, 0, 0);
        }

        private void AutoType()
        {
            var pressDelayTime = int.Parse(delayTime.Text);
            var keyToPress = buttonToPress.Text; // Capture UI value before entering loop
            var pressDuration = pressedFor.Text;
            
            while (_pressKey)
            {
                DoType(keyToPress, pressDuration);
                Thread.Sleep(pressDelayTime);
            }
        }

        private (uint x, uint y) GetCursorPosition()
        {
            var x = (uint)Cursor.Position.X;
            var y = (uint)Cursor.Position.Y;

            return (x, y);
        }

        /// <summary>
        /// Simulates a click at the cursor's current location
        /// </summary>
        private void DoMouseClick()
        {
            var isMouseInsideRobloxWindow = Win32.IsMouseInsideRobloxWindow(chkInRobloxOnly.Checked);

            var cursorPosition = GetCursorPosition();
            var x = cursorPosition.x;
            var y = cursorPosition.y;

            if (!isMouseInsideRobloxWindow)
                return;   
            
            if (_keepPressed)
            {
                Win32.mouse_event(Win32.MouseEventLeftDown, x, y, 0, 0);
                if (DateTime.Now.Subtract(_lastRightClickTime).TotalMinutes >= RightClickDelayMinutes)
                {
                    Win32.mouse_event(Win32.MouseEventRightDown | Win32.MouseEventRightUp, x, y, 0, 0);
                    _lastRightClickTime = DateTime.Now;
                }
            }
            else
            {
                Win32.mouse_event(Win32.MouseEventLeftDown | Win32.MouseEventLeftUp, x, y, 0, 0);
            }
        }

        private static Keys ParseKeyString(string keyString)
        {
            if (string.IsNullOrWhiteSpace(keyString))
                return Keys.None;

            // Convert to uppercase for case-insensitive comparisonw
            var upperKey = keyString.ToUpper().Trim();

            // Single character letters (A-Z)
            if (upperKey.Length == 1 && upperKey[0] >= 'A' && upperKey[0] <= 'Z')
                return (Keys)Enum.Parse(typeof(Keys), "A") + (upperKey[0] - 'A');

            // Single character numbers (0-9)
            if (upperKey.Length == 1 && upperKey[0] >= '0' && upperKey[0] <= '9')
            {
                if (upperKey[0] == '0')
                    return Keys.D0;
                return (Keys)Enum.Parse(typeof(Keys), "D1") + (upperKey[0] - '1');
            }

            // Function keys
            if (upperKey.StartsWith("F") && upperKey.Length >= 2)
            {
                if (int.TryParse(upperKey.Substring(1), out int fNum) && fNum >= 1 && fNum <= 24)
                    return (Keys)Enum.Parse(typeof(Keys), $"F{fNum}");
            }

            // Common key names mapping
            switch (upperKey)
            {
                case "ENTER": return Keys.Enter;
                case "RETURN": return Keys.Enter;
                case "SPACE": return Keys.Space;
                case "SPACEBAR": return Keys.Space;
                case "TAB": return Keys.Tab;
                case "BACKSPACE": return Keys.Back;
                case "BACK": return Keys.Back;
                case "DELETE": return Keys.Delete;
                case "DEL": return Keys.Delete;
                case "INSERT": return Keys.Insert;
                case "INS": return Keys.Insert;
                case "HOME": return Keys.Home;
                case "END": return Keys.End;
                case "PAGEUP": return Keys.PageUp;
                case "PAGEDOWN": return Keys.PageDown;
                case "UP": return Keys.Up;
                case "DOWN": return Keys.Down;
                case "LEFT": return Keys.Left;
                case "RIGHT": return Keys.Right;
                case "ESC": return Keys.Escape;
                case "ESCAPE": return Keys.Escape;
                case "PAUSE": return Keys.Pause;
                case "BREAK": return Keys.Pause;
                case "PRINTSCREEN": return Keys.PrintScreen;
                case "PRTSC": return Keys.PrintScreen;
                case "SCROLLLOCK": return Keys.Scroll;
                case "CAPSLOCK": return Keys.CapsLock;
                case "NUMLOCK": return Keys.NumLock;
                case "CTRL": return Keys.ControlKey;
                case "CONTROL": return Keys.ControlKey;
                case "SHIFT": return Keys.ShiftKey;
                case "ALT": return Keys.Menu;
                case "APPS": return Keys.Apps;
                case "WINDOWS": return Keys.LWin;
                case "WIN": return Keys.LWin;
            }

            // Try direct enum parsing as fallback
            try
            {
                return (Keys)Enum.Parse(typeof(Keys), upperKey, true);
            }
            catch
            {
                return Keys.None;
            }
        }

        /// <summary>
        ///     Simulates a click at the cursor's current location
        /// </summary>
        private void DoType(string keyToPress, string pressDuration)
        {
            var isMouseInsideRobloxWindow = Win32.IsMouseInsideRobloxWindow(chkInRobloxOnly.Checked);

            if (!_pressKey || !isMouseInsideRobloxWindow) return;

            var key = ParseKeyString(keyToPress);
            
            if (key == Keys.None)
            {
                // Invalid key - silently return or could show error
                return;
            }
            var pressActiveTime = int.Parse(pressDuration);

            _globalKeyboardHook.SendKeys(key, true);

            Thread.Sleep(pressActiveTime);

            _globalKeyboardHook.SendKeys(key, false);
        }
        
        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _minTimeDefault = 2500;
            _maxTimeDefault = 3000;
            minWait.Text = _minTimeDefault.ToString();
            maxWait.Text = _maxTimeDefault.ToString();
            fastToolStripMenuItem.CheckState = CheckState.Unchecked;
            slowToolStripMenuItem.CheckState = CheckState.Checked;
            _fastMode = false;
        }

        private void fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _minTimeDefault = 25;
            _maxTimeDefault = 30;
            minWait.Text = _minTimeDefault.ToString();
            maxWait.Text = _maxTimeDefault.ToString();
            fastToolStripMenuItem.CheckState = CheckState.Checked;
            slowToolStripMenuItem.CheckState = CheckState.Unchecked;
            _fastMode = true;
        }

        private void clickAnywhereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chkInRobloxOnly.Checked = !chkInRobloxOnly.Checked;
            clickAnywhereToolStripMenuItem.Checked = !clickAnywhereToolStripMenuItem.Checked;
        }

        private void keepPressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleKeepPressed();
        }

        #endregion
        
        #region Enable/Disable Form Input Fields

        /// <summary>
        ///     Allows the user to click the startButton,
        ///     maxClicksBetweenMovement text field,
        ///     minClicksBetweenMovement text field,
        ///     minWait text field,
        ///     maxWait text field.
        /// </summary>
        private void EnableSettingFields()
        {
            _clickMouse = false;
            mnuStart.Enabled = true;
            mnuStop.Enabled = false;
            minWait.Enabled = true;
            maxWait.Enabled = true;
        }

        /// <summary>
        ///     Disables the fields so user can't edit
        ///     maxClicksBetweenMovement text field,
        ///     minClicksBetweenMovement text field,
        ///     minWait text field,
        ///     maxWait text field.
        /// </summary>
        private void DisableSettingFields()
        {
            mnuStart.Enabled = false;
            mnuStop.Enabled = true;
            minWait.Enabled = false;
            maxWait.Enabled = false;
        }

        #endregion

        private void chkKeepPressed_CheckedChanged(object sender, EventArgs e)
        {
            _keepPressed = chkKeepPressed.Checked;
        }
    }
}