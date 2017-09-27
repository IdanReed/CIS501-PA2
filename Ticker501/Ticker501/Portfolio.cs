///Jarod Honas
///501SF17
///A1
///Portfolio
///Holds stocks under a name and is able to return its total cash value and its number of stocks held
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Portfolio
    {
        public List<Stock> stocks;
        string name;
        double cashValue = 0;
        int amountStocks = 0;

        public Portfolio()
        {
            stocks = new List<Stock>();
        }

        public int AmountStocks
        {
            get {
                int count = 0;
                foreach(Stock s in stocks)
                {
                    count += s.Amount;
                }
                amountStocks = count;
                return amountStocks; }
            set { amountStocks = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double CashValue
        {
            get {
                double count = 0;
                foreach(Stock s in stocks)
                {
                    count += s.TotalPrice;
                }
                cashValue = count;
                return count;
                }
            
        }

        public override string ToString()
        {
            string temp = "";
            temp += "Portfolio: " + name + "\n";
            foreach (Stock s in stocks)
            {
                temp += "\t"+s.ToString();
                temp += "\n\t\tGains/Losses: ";
                if (s.TotalPrice < s.InitPrice) temp += "-";
                temp += (s.TotalPrice - s.InitPrice).ToString("C") + "\n\t\tPrice When Bought: [" + s.InitPrice.ToString("C") + "]\n";
            }
            return temp;
        }

    }
}
