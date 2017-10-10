/// <summary>
/// Account
/// Holds all of the values for the account including its portfolios, funds, tickers, simulator, value
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
    public class Account
    {
        private double _funds = 0;//account funds
        private List<Ticker> _tickers = new List<Ticker>();//list of Tickers read from file
        private List<Portfolio> _portfolios = new List<Portfolio>();//list of portfolios held by account
        private Simulation _sim = new Simulation("low");//simulation instance with an initial stability of low
        private double _depositFees = 0;//Amount of fees accrued from deposits
        private double _tradeFees = 0;//amount of fees accrued from trades
        private double _curValue = 0;//current value of the account
        
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
                /*
                double portValue = 0;
                foreach(Portfolio p in _portfolios)
                {
                    portValue += p.CashValue;
                }

                return _curValue + portValue;*/
                return _curValue;
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
                foreach(Portfolio p in _portfolios)
                {
                    curValue += p.ChangeInValue;
                }
                return curValue - DepositFees;
            }
        }

        #endregion Getters/Setters
    }
}
