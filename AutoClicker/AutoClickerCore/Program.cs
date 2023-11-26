using System;
using System.Windows.Forms;

namespace AutoClickerCore
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            var autoClicker = new AutoClicker();
            Application.Run(autoClicker);
        }
    }
}