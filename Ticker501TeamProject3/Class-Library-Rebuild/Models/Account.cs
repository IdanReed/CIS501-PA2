﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Account_M
    {
        public const int MAX_NUMER_OF_PORTFOLIOS = 3;
        private double _funds = 0;
        private List<Transaction> _transactions = new List<Transaction>();
        private List<Portfolio_M> _portfolios = new List<Portfolio_M>();

        
        /// <summary>
        /// Getter for funds in the account
        /// </summary>
        public double Funds
        {
            get
            {
                return _funds;

            }
        }

        /// <summary>
        /// Getter for the list of transactions in the account
        /// </summary>
        public List<Transaction> Transactions
        {
            get { return _transactions; }
        }

        /// <summary>
        /// Getter for the list of portfolios in the account
        /// </summary>
        public List<Portfolio_M> Portfolios
        {
            get { return _portfolios; }
        }

        /// <summary>
        /// Deposit a given amount of money in the account, as well as a total deposit fee
        /// </summary>
        /// <param name="amount">Amount to deposit</param>
        public void Deposit(double amount)
        {
            _funds += amount + Fee_M.DEPOSIT;
            _transactions.Add(new Fee_M(Fee_M.FeeSelect.DepositOrWithdraw));
        }

        /// <summary>
        /// Withdraws a given amount of money plus a deposit/withdraw fee, given there is enough money in the account
        /// </summary>
        /// <param name="amount">Amount to withdraw</param>
        public void Withdraw(double amount)
        {
            if(_funds >= amount + Fee_M.DEPOSIT)
            {
                _funds -= amount;
                _funds += Fee_M.DEPOSIT;

                _transactions.Add(new Fee_M(Fee_M.FeeSelect.DepositOrWithdraw)); 
            }
            else
            {
                throw new ArgumentException("Withdrawing too much money");
            }
        }

        /// <summary>
        /// Calculates the value of all stocks held in each portfolio to get the total value of the account
        /// </summary>
        /// <param name="stockList">Current list of stocks</param>
        /// <returns>Total value of account</returns>
        public double CalculateValue(List<Stock_M> stockList)
        {
            double sum = 0;
            foreach (Portfolio_M portfolio in _portfolios)
            {
                sum += portfolio.HeldValueCurrent(stockList);
            }
            sum += _funds;
            return sum;
        }

        /// <summary>
        /// Calculates the gain/loss for the account
        /// </summary>
        /// <param name="stockList">Current list of all stocks</param>
        /// <returns>Total gain/loss</returns>
        public double CalculateGainLoss(List<Stock_M> stockList)
        {
            double sum = 0;
            foreach (Portfolio_M portfolio in _portfolios)
            {
                sum += portfolio.GainLoss();
            }
            foreach(Transaction trans in _transactions)
            {
                sum += trans.GainLossInfluence;
            }
            return sum;
        }


        /// <summary>
        /// Deletes a portfolio from the account
        /// </summary>
        /// <param name="name">Name of portfolio to be deleted</param>
        public void DeletePortfolio(string name, List<Stock_M> stocks)
        {
            Portfolio_M selectedPortfolio = GetPortfolioByName(name);
            
            foreach(BuyOrSell_M BOS in selectedPortfolio.CurrentlyHeld())
            {
                Stock_M stock = stocks.Find(s => BOS.StockName == s.Name);
                selectedPortfolio.SellStock(stock, BOS.Quantity);
            }

            _transactions.AddRange(selectedPortfolio.TransactionList);
            if (_portfolios.RemoveAll((p) => p.Name == name) == 0)
            {
                throw new ArgumentException("No portfolio exists with that name.");
            }
        }

        /// <summary>
        /// Creates a portfolio for use in the account
        /// </summary>
        /// <param name="name">Name of the portfolio to be created</param>
        /// <param name="ver">Stock Verifier delegate for the portfolio</param>
        /// <returns>Boolean saying if the creation was completed</returns>
        public void CreatePortfolio(string name, StockVerifier ver)
        {
            bool flag = false;
            foreach(Portfolio_M p in Portfolios)
            {
                if(p.Name == name)
                {
                    flag = true;
                }
            }
            if (_portfolios.Count >= MAX_NUMER_OF_PORTFOLIOS) throw new ArgumentException("Max number of portfolios reached.");
            if (_portfolios.Exists(p => p.Name == name)) throw new ArgumentException("A portfolio with that name already exists");

            _portfolios.Add(new Portfolio_M(name, ver, ManageFunds));

        }

        /// <summary>
        /// Finds a portfolio by its name
        /// </summary>
        /// <param name="name">Name of portfolio to find</param>
        /// <returns>The portfolio that was found</returns>
        public Portfolio_M GetPortfolioByName(string name)
        {
            return _portfolios.Find((p) => p.Name == name);
        }

        /// <summary>
        /// Used for StockVerifier delegate
        /// </summary>
        /// <param name="type">Either a buy or sell transaction</param>
        /// <param name="cost">Total cost of transaction</param>
        /// <returns>Boolean of whether or not the buy/sell was complete</returns>
        private bool ManageFunds(BuyOrSell_M.BuyOrSellEnum type, double cost, double fee)
        {
            _funds += fee;

            if (BuyOrSell_M.BuyOrSellEnum.Sell == type)
            {
                _funds += cost;
                return true;
            }else if(BuyOrSell_M.BuyOrSellEnum.Buy == type)
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
