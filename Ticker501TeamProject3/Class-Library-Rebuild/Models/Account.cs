using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Account
    {
        public const int MAX_NUMER_OF_PORTFOLIOS = 3;
        private double _funds = 0;
        private List<Transaction> _transactions = new List<Transaction>();
        private List<Portfolio> _portfolios = new List<Portfolio>();

        public double Funds
        {
            get { return _funds; }
        }
        public List<Transaction> Transactions
        {
            get { return _transactions; }
        }
        public List<Portfolio> Portfolios
        {
            get { return _portfolios; }
        }
        public void Deposit(double amount)
        {
            _funds += amount + Fee.DEPOSIT;
            _transactions.Add(new Fee(Fee.FeeSelect.DepositOrWithdraw));
        }
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
            if (_portfolios.RemoveAll((p) => p.Name == name) == 0)
            {
                throw new ArgumentException("No portfolio exists with that name.");
            }
        }
        public bool CreatePortfolio(string name, StockVerifier ver)
        {
            bool flag = false;
            foreach(Portfolio p in Portfolios)
            {
                if(p.Name == name)
                {
                    flag = true;
                }
            }
            if (_portfolios.Count < MAX_NUMER_OF_PORTFOLIOS && )
            {
                _portfolios.Add(new Portfolio(name, ver, ManageFunds));
                return true;
            }
            else
            {
                return false;
            }

        }
        public Portfolio GetPortfolioByName(string name)
        {
            return _portfolios.Find((p) => p.Name == name);
        }
        private bool ManageFunds(BuyOrSell.BuyOrSellEnum type, double cost)
        {
            if(BuyOrSell.BuyOrSellEnum.Sell == type)
            {
                _funds += cost;
                return true;
            }else if(BuyOrSell.BuyOrSellEnum.Buy == type)
            {
                if(_funds >= cost)
                {
                    _funds -= cost;
                    return true;
                }
            }
            return false;
        }
        
    }
}
