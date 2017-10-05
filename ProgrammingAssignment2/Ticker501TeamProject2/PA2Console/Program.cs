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
        static void Main(string[] args)
        {
            List<Ticker> tickers = GetTickers();
            Account acct = new Account();
            Controller c = new Controller(acct, tickers);
            InputView inputView = new InputView(c.InputHandle);
            OutputView outputView = new OutputView(acct, tickers);

            c.AddListener(outputView.Update);
            inputView.Start();
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
