using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class MainModel
    {
        private Account _account = new Account();
        private List<Stock> _stocks = new List<Stock>();

        public Account Account
        {
            get { return _account; }
        }
        public List<Stock> Stocks
        {
            get { return _stocks; }
        }
        public MainModel()
        {
        }
        public void LoadTickersFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines("Ticker.txt");
            foreach (string line in lines)
            {
                if (line != "")
                {
                    string[] parts = line.Split('-');

                    string name = parts[1];
                    string tag = parts[0];
                    double price = Convert.ToDouble(parts[2].Substring(1));

                    Stock stock = new Stock(name, tag, price);
                    _stocks.Add(stock);
                }
            }
        }
        public bool VerifyStock(Stock stock)
        {
            return _stocks.Contains(stock);
            //return stocks.Find((s) => s.Equals(stock)) != null;
        }
        
    }
}
