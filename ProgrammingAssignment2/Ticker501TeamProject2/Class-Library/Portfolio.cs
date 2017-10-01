using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Portfolio
    {
        private List<StockPurchase> _stocks;
        private string _name;
        private double _cashValue = 0;
        private int _amountStocks = 0;
        

        public Portfolio(string name)
        {
            _name = name;
            _stocks = new List<StockPurchase>();
        }

        public int AmountStocks
        {
            get {
                int count = 0;
                foreach(StockPurchase s in _stocks)
                {
                    count += s.Amount;
                }
                _amountStocks = count;
                return _amountStocks; }
            set { _amountStocks = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<StockPurchase> Stocks
        {
            get
            {
                return _stocks;
            }
            set
            {
                _stocks = value;
            }
        }

        public double CashValue
        {
            get {
                double count = 0;
                foreach(StockPurchase s in _stocks)
                {
                    count += s.TotalPrice;
                }
                _cashValue = count;
                return count;
                }
            
        }

        public override string ToString()
        {
            string temp = "";
            temp += "Portfolio: " + _name + "\n";
            foreach (StockPurchase s in _stocks)
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
