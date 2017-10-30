using ModelRebuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticker501TeamProject3
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
            MainModel mm = new MainModel();
            Controller c = new Controller(mm);

            MainGUI gui = new MainGUI(c.EventHandle, mm);

            c.RegisterObserver(gui.EventHandler.EventHandle);
            Application.Run(gui);
           // Application.Run(new MainGui());
        }
    }
}
