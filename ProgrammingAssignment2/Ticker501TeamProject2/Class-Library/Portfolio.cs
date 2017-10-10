/// <summary>
/// Portfolio
/// Holds all of the stocks in the portfolio as well as its name
/// </summary>
/// <param name="price"></param>
/// <returns></returns>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Portfolio
    {
        private List<StockPurchase> _stocks;//List of stocks held in the portfolio
        private string _name;//name given to the portfolio
        private double _cashValue = 0;//cash value of all of the stocks in the portfolio
        private int _amountStocks = 0;//total amount of stocks held
        private double _changeInValue = 0;//Change in value over time in the portfolio
        private double _totalFees = 0;//total amount of fees accrued from trades

        
        /// <summary>
        /// PortfolioConstructor
        /// Takes a name for the portfolio and initializes the List of stocks
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public Portfolio(string name)
        {
            _name = name;
            _stocks = new List<StockPurchase>();
        }

        /*
         * Getters/Setters
         */
        #region Getters/Setters
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

        public Double ChangeInValue
        {
            get
            {
                double initValue = 0;
                foreach(StockPurchase sp in Stocks)
                {
                    initValue += sp.InitPrice;
                }
                
                return (CashValue - initValue - _totalFees);
            }
           
        }

        public Double TotalFees
        {
            get
            {
                return _totalFees;
            }
            set
            {
                _totalFees += value;
            }
        }
        #endregion


        
        /// <summary>
        /// GetInitValue
        /// Gets the initial value of all of the stocks for comparison to current values
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public double GetInitValue()
        {
            double initVal = 0;
            foreach(StockPurchase sp in _stocks)
            {
                initVal += sp.InitPrice;
            }
            return initVal;
         }

        
        /// <summary>
        /// ToString
        /// Returns the portfolios statistics in string format
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
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
