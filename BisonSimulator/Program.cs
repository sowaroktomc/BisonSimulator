using System;
using System.Net;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 5;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MultiProfitSimForm());
            //Application.Run(new CleanupForm());
        }
    }
}
