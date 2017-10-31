using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{

    /// <summary>
    /// Holds the account and the list of company stocks
    /// Loads the stocks in from the file and puts it into a list
    /// </summary>
    public class MainModel
    {
        private Account _account = new Account();
        private List<Stock> _stocks = new List<Stock>();

        /// <summary>
        /// Getter for the account
        /// </summary>
        public Account Account
        {
            get { return _account; }
        }

        /// <summary>
        /// Getter for the list of stocks
        /// </summary>
        public List<Stock> Stocks
        {
            get { return _stocks; }
        }
        

        /// <summary>
        /// Gets the name, tag, and price for all the companies in the text file and puts them into stocks
        /// </summary>
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

        /// <summary>
        /// Returns whether the stock is contained in the list of stocks
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public bool VerifyStock(Stock stock)
        {
            return _stocks.Contains(stock);
            //return stocks.Find((s) => s.Equals(stock)) != null;
        }
        
    }
}
