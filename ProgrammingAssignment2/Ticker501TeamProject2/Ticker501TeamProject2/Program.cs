using Class_Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticker501TeamProject2
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
           
            
            List<Ticker> tickers = GetTickers();
            Account acct = new Account();
            Simulation sim = new Simulation("low");
            
            Controller c = new Controller(acct, tickers, sim);

            MainGUI form = new MainGUI(acct, tickers, c.InputHandle);

            c.AddListener(form.Update);
        }
        static List<Ticker> GetTickers()
        {
            StreamReader sr = new StreamReader("ticker.txt");
            string line = sr.ReadLine();
            List<Ticker> toReturn = new List<Ticker>();
            while (line != null)
            {
                string[] split = line.Split('-');
                double tempPrice = Convert.ToDouble(split[2].Substring(1));
                toReturn.Add(new Ticker(split[0], split[1], tempPrice));
                line = sr.ReadLine();
            }
            return toReturn;
        }
    }
}
