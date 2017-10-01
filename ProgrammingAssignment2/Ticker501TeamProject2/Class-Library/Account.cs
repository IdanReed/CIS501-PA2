using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Account
    {
        private double _funds = 0;
        private List<Ticker> _tickers = new List<Ticker>();
        private List<Portfolio> _portfolios = new List<Portfolio>();
        private Simulation _sim = new Simulation("low");
        
        #region Getters/Setters
        public double Funds
        {
            get
            {
                return _funds;
            }
            set
            {
                _funds = value;
            }
        }
        public List<Ticker> Tickers
        {
            get
            {
                return _tickers;
            }
        }
        public List<Portfolio> Portfolios
        {
            get
            {
                return _portfolios;
            }
            set
            {
                _portfolios = value;
            }
        }

        #endregion Getters/Setters
    }
}
