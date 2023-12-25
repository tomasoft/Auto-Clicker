using System;
using System.Threading;
using System.Windows.Forms;


namespace AutoClicker
{
    public partial class AutoClicker : Form
    {
        #region Variables

        private const int MinTimeDefault = 45;
        private const int MaxTimeDefault = 55;
        private const int DelayTime = 5000;
        private const int PressedFor = 500;
        private const string ButtonToPress = "E";
        
        private bool _clickMouse;
        private bool _pressKey;

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
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState != GlobalKeyboardHook.KeyboardState.KeyDown) return;

            if (e.KeyboardData.VirtualCode.Equals((int)Keys.F8))
            {
                ToggleAutoClicker(!_clickMouse);
                if (e.KeyboardData.IsControlPressed)
                    ToggleAutoType(!_pressKey);
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
            minWait.Text = $@"{MinTimeDefault}";
            maxWait.Text = $@"{MaxTimeDefault}";
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
                    AutoClickOnNewThread();
                    _clickMouse = true;
                    DisableSettingFields();
                }
                else
                {
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
            var minWaitTime = int.Parse(minWait.Text);
            var maxWaitTime = int.Parse(maxWait.Text);

            while (_clickMouse)
            {
                var rnd = new Random();
                var timeBetweenClicks = rnd.Next(minWaitTime, maxWaitTime);
                DoMouseClick();
                Thread.Sleep(timeBetweenClicks);
            }
        }

        private void AutoType()
        {
            var pressDelayTime = int.Parse(delayTime.Text);
            
            while (_pressKey)
            {
                DoType();
                Thread.Sleep(pressDelayTime);
            }
        }

        /// <summary>
        ///     Simulates a click at the cursor's current location
        /// </summary>
        private void DoMouseClick()
        {
            var isMouseInsideRobloxWindow = Win32.IsMouseInsideRobloxWindow(chkInRobloxOnly.Checked);

            if (!_clickMouse || !isMouseInsideRobloxWindow) return;

            //Call the imported function with the cursor's current position
            var x = (uint) Cursor.Position.X;
            var y = (uint) Cursor.Position.Y;
            Win32.mouse_event(Win32.MouseEventLeftDown | Win32.MouseEventLeftUp, x, y, 0, 0);
        }

        /// <summary>
        ///     Simulates a click at the cursor's current location
        /// </summary>
        private void DoType()
        {
            var isMouseInsideRobloxWindow = Win32.IsMouseInsideRobloxWindow(chkInRobloxOnly.Checked);

            if (!_pressKey || !isMouseInsideRobloxWindow) return;

            var key = (Keys)Enum.Parse(typeof(Keys), buttonToPress.Text);
            var pressActiveTime= int.Parse(pressedFor.Text);
            
            //SendKeys.SendWait("E");


            //isim.Keyboard.Sleep(pressActiveTime);
            //isim.Keyboard.KeyUp(VirtualKeyCode.VK_E);

            _globalKeyboardHook.SendKeys(key, true);
            Thread.Sleep(pressActiveTime);
            _globalKeyboardHook.SendKeys(key, false);
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
    }
}