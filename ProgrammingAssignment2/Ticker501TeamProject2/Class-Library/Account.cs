using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Account
    {
        private static double _funds = 0;
        private static List<Ticker> _tickers = new List<Ticker>();
        private static List<Portfolio> _portfolios = new List<Portfolio>();
        private static Simulation sim = new Simulation("low");
        
        #region Getters/Setters
        public static double Funds
        {
            get
            {
                return _funds;
            }
        }
        public static List<Ticker> Tickers
        {
            get
            {
                return _tickers;
            }
        }
        public static List<Portfolio> Portfolios
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

        public static void Deposit(double amt)
        {
            _funds += amt;
        } 
        public static void Withdraw(double amt)
        {
            _funds -= amt;
        }
    }
}
