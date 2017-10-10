using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PA2Console
{
    using Class_Library;
    class Program
    {
        /// <summary>
        /// The main entry point for the console project
        /// </summary>
        /// <param name="args">The args passed in from the command line</param>
        static void Main(string[] args)
        {
            List<Ticker> tickers = GetTickers();
            Account acct = new Account();
            Simulation sim = new Simulation("low");

            Controller c = new Controller(acct, tickers, sim);
            InputView inputView = new InputView(c.InputHandle);
            OutputView outputView = new OutputView(acct, tickers);

            c.AddListener(outputView.Update);
            inputView.Start();
        } 

        /// <summary>
        /// Gets the tickers from the "ticker.txt" file
        /// </summary>
        /// <returns>The list of tickers</returns>
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
