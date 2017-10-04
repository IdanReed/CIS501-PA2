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
            while (line != "")
            {
                //Finish this once format is figured out
            }
            //Temporary so it builds
            return new List<Ticker>();
        }
    }
}
