using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class MainModel
    {
        public Account account = new Account();
        public List<Stock> stocks = new List<Stock>();

        public MainModel()
        {
        }
        public void LoadTickersFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines("Ticker.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('-');

                string name = parts[1];
                string tag = parts[0];
                double price = Convert.ToDouble(parts[2].Substring(1));

                Stock stock = new Stock(name, tag, price);
                stocks.Add(stock);
            }
        }
        public bool VerifyStock(Stock stock)
        {
            return stocks.Contains(stock);
            //return stocks.Find((s) => s.Equals(stock)) != null;
        }
    }
}
