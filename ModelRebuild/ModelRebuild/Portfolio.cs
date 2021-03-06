﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public delegate bool StockVerifier(Stock stock);
    public class Portfolio
    {
        private List<Transaction> _transactionList = new List<Transaction>();
        private StockVerifier _verifier;
        public readonly string Name;

        public List<Transaction> TransactionList
        {
            get { return _transactionList; }
        }

        public Portfolio(string nameIn, StockVerifier ver)
        {
            Name = nameIn;
            _verifier = ver;
        }
        public double GainLoss(List<Stock> stockList)
        {
            double gainLossSum = 0;
            foreach (Transaction transaction in _transactionList)
            {
                gainLossSum += transaction.GainLossInfluence;
            }

            gainLossSum += Value(stockList);

            return gainLossSum;
        }
        public double Value(List<Stock> stockList)
        {
            List<Tuple<string, int>> HeldStockList = new List<Tuple<string, int>>();

            foreach (Transaction curTrans in _transactionList)
            {

                if (curTrans.GetType() == typeof(BuyOrSell))
                {
                    BuyOrSell curBOS = (BuyOrSell)curTrans;

                    Tuple<string, int> createdTuple = null;
                    Tuple<string, int> foundTuple = null;
                    foreach (Tuple<string, int> heldTuple in HeldStockList)
                    {
                        if (curBOS.StockName == heldTuple.Item1)
                        {
                            if (curBOS.BuyOrSellState == BuyOrSell.BuyOrSellEnum.Buy)
                            {
                                createdTuple = Tuple.Create(heldTuple.Item1, heldTuple.Item2 + curBOS.Quantity);

                            }
                            else
                            {
                                createdTuple = Tuple.Create(heldTuple.Item1, heldTuple.Item2 - curBOS.Quantity);

                            }
                            break;
                        }
                    }
                    if (createdTuple != null)
                    {
                        HeldStockList[HeldStockList.IndexOf(foundTuple)] = createdTuple;
                    }
                    else
                    {
                        HeldStockList.Add(createdTuple);
                    }

                }

            }
            double sum = 0;
            foreach (Tuple<string, int> heldStock in HeldStockList)
            {
                double heldStockPrice;
                foreach (Stock stock in stockList)
                {
                    if (heldStock.Item1 == stock.Name)
                    {
                        sum += heldStock.Item2 * stock.Price;
                    }
                }
            }

            return sum;
        }

        public void PurchaseStock(Stock stock, int amount)
        {
            if (_verifier(stock))
            {
                BuyOrSell buyOrSell = new BuyOrSell(stock, amount, BuyOrSell.BuyOrSellEnum.Buy);
            }
        }
    }
}
