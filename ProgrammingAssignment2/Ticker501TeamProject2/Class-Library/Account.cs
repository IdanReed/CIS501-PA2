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
        private double _depositFees = 0;
        private double _tradeFees = 0;
        private double _curValue = 0;
        
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

        public double DepositFees
        {
            get
            {
                return _depositFees;
            }
            set
            {
                _depositFees = value;
            }
        }

        public double TradeFees
        {
            get
            {
                return _tradeFees;
            }
            set
            {
                _tradeFees = value;
            }
        }

        public double CurValue
        {
            get
            {
                double portValue = 0;
                foreach(Portfolio p in _portfolios)
                {
                    portValue += p.CashValue;
                }

                return _curValue + portValue;
            }
            set
            {
                _curValue = value;
            }
        }

        public double ChangeInFunds
        {
            get
            {
                
                double curValue = 0;
                double initValue = 0;
                foreach(Portfolio p in _portfolios)
                {
                    curValue += p.CashValue + p.ChangeInValue;
                    initValue += p.GetInitValue();
                }
                return (_funds + (curValue-initValue)) - _curValue;
            }
        }

        #endregion Getters/Setters
    }
}
