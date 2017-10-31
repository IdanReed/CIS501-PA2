using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public delegate bool StockVerifier(Stock stock);
    public delegate bool FundsVerifier(BuyOrSell.BuyOrSellEnum type, double cost);
    public class Portfolio
    {
        private List<Transaction> _transactionList = new List<Transaction>();
        private StockVerifier _verifierStock;
        private FundsVerifier _fundsManager;
        public readonly string Name;

        public List<Transaction> TransactionList
        {
            get { return _transactionList; }
        }

        public Portfolio(string nameIn, StockVerifier verifierStock, FundsVerifier fundsManager)
        {
            Name = nameIn;
            _verifierStock = verifierStock;
            _fundsManager = fundsManager;
        }
        public double GainLoss(List<Stock> stockList)
        {
            double gainLossSum = 0;
            foreach (Transaction transaction in _transactionList)
            {
                gainLossSum += transaction.GainLossInfluence;
            }

            gainLossSum += HeldValueCurrent(CurrentlyHeld(), stockList);

            return gainLossSum;
        }
        public double HeldValueCurrent(List<Tuple<string, double, int>> heldList, List<Stock> stockList)
        {
            double sum = 0;
            foreach(Tuple<string, double, int> curTuple in heldList)
            {
                foreach(Stock stock in stockList)
                {
                    if(stock.Name == curTuple.Item1)
                    {
                        sum += stock.Price * curTuple.Item3;
                    }
                }
            }
            return sum;
        }
        public double HeldValueAtPurchase()
        {
            List<Tuple<string, double, int>> historicHeld = CurrentlyHeld();

            double sum = 0;
            foreach (Tuple<string, double, int> curTuple in historicHeld)
            {
                sum += curTuple.Item2 * curTuple.Item3;
            }
            return sum;
        }
        public List<Tuple<string, double, int>> CurrentlyHeld()
        {
            List<Tuple<string, double, int>> HeldStockList = new List<Tuple<string, double, int>>();

            foreach (Transaction curTrans in _transactionList)
            {

                if (curTrans.GetType() == typeof(BuyOrSell))
                {
                    BuyOrSell curBOS = (BuyOrSell)curTrans;

                    Tuple<string, double, int> createdTuple = null;
                    Tuple<string, double, int> foundTuple = null;
                    foreach (Tuple<string, double, int> heldTuple in HeldStockList)
                    {
                        if (curBOS.StockName == heldTuple.Item1)
                        {
                            if (curBOS.BuyOrSellState == BuyOrSell.BuyOrSellEnum.Buy)
                            {
                                createdTuple = Tuple.Create(heldTuple.Item1, heldTuple.Item2, (int)heldTuple.Item3 + curBOS.Quantity);

                            }
                            else
                            {
                                createdTuple = Tuple.Create(heldTuple.Item1, heldTuple.Item2, (int)heldTuple.Item3 - curBOS.Quantity);

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
            return HeldStockList;
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
            if (_verifierStock(stock))
            {
                if(_fundsManager(BuyOrSell.BuyOrSellEnum.Buy, (stock.Price * amount + Fee.BUY_OR_SELL)))
                {
                    BuyOrSell buyOrSell = new BuyOrSell(stock, amount, BuyOrSell.BuyOrSellEnum.Buy);
                }
                else
                {
                    throw new ArgumentException("Not enough cash");
                }
            }
        }
        public void SellStock(Stock stock, int amount)
        {
            if (_verifierStock(stock))
            {
                if (_fundsManager(BuyOrSell.BuyOrSellEnum.Sell, (stock.Price * amount + Fee.BUY_OR_SELL)))
                {
                    BuyOrSell buyOrSell = new BuyOrSell(stock, amount, BuyOrSell.BuyOrSellEnum.Sell);

                }
                else
                {
                    throw new ArgumentException("Not enough cash");
                }
            }
        }
    }
}
