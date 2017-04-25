using System;
using System.Windows.Forms;

namespace CPMClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new ClientWindow();
            Application.Run(MainWindow);
        }

        public static ClientWindow MainWindow;
    }
}
