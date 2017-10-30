using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Account
    {
        private double _funds = 0;
        private List<Transaction> _transactions = new List<Transaction>();

        public double Funds
        {
            get { return _funds; }
        }
        public List<Transaction> Transactions
        {
            get { return _transactions; }
        }
        private List<Portfolio> _portfolios = new List<Portfolio>();
        public List<Portfolio> Portfolios
        {
            get { return _portfolios; }
        }
        
        public void Deposit(double amount)
        {
            _funds += amount + Fee.DEPOSIT;
            _transactions.Add(new Fee(Fee.FeeSelect.DepositOrWithdraw));
        }

        /// <summary>
        /// Withdraws an amount of money from the account
        /// </summary>
        /// <param name="amount">The amount to withdraw</param>
        /// <exception cref="ArgumentException"></exception>
        public void Withdraw(double amount)
        {
            if(_funds >= amount - Fee.DEPOSIT)
            {
                _funds -= amount;
                _funds += Fee.DEPOSIT;

                _transactions.Add(new Fee(Fee.FeeSelect.DepositOrWithdraw)); 
            }
            else
            {
                throw new ArgumentException("Withdrawing too much money");
            }
        }
        public double CalculateValue(List<Stock> stockList)
        {
            double sum = 0;
            foreach (Portfolio portfolio in _portfolios)
            {
                sum += portfolio.Value(stockList);
            }
            sum += _funds;
            return sum;
        }
        public double CalculateGainLoss(List<Stock> stockList)
        {
            double sum = 0;
            foreach (Portfolio portfolio in _portfolios)
            {
                sum += portfolio.GainLoss(stockList);
            }
            foreach(Transaction trans in _transactions)
            {
                sum += trans.GainLossInfluence;
            }
            return sum;
        }
        public void DeletePortfolio(int portNum)
        {
            if(portNum < _portfolios.Count)
            {
                _portfolios[portNum] = null;
            }
        }
        public void DeletePortfolio(string name)
        {
            _portfolios.RemoveAll((p) => p.Name == name);
        }
        public bool CreatePortfolio(string name, StockVerifier ver)
        {
            if (_portfolios.Count > 2)
            {
                return false;
            }
            else
            {
                _portfolios.Add(new Portfolio(name, ver));
                return true;
            }

        }
        public Portfolio GetPortfolioByName(string name)
        {
            return _portfolios.Find((p) => p.Name == name);
        }
    }
}
