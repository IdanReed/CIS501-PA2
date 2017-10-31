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

            gainLossSum += HeldValueCurrent(stockList);

            return gainLossSum;
        }
        public double HeldValueCurrent(List<Stock> stockList)
        {
            List<BuyOrSell> heldList = CurrentlyHeld();
            double sum = 0;
            foreach(BuyOrSell curBOS in heldList)
            {
                foreach(Stock stock in stockList)
                {
                    if(stock.Name == curBOS.StockName)
                    {
                        sum += stock.Price * curBOS.Quantity;
                    }
                }
            }
            return sum;
        }
        public double HeldValueAtPurchase()
        {
            List<BuyOrSell> historicHeld = CurrentlyHeld();

            double sum = 0;
            foreach (BuyOrSell curBOS in historicHeld)
            {
                sum += curBOS.Quantity * curBOS.PricePerStock;
            }
            return sum;
        }
        public List<BuyOrSell> CurrentlyHeld()
        {
            List<BuyOrSell> HeldStockList = new List<BuyOrSell>();

            foreach (Transaction curTrans in _transactionList)
            {

                if (curTrans.GetType() == typeof(BuyOrSell))
                {
                    BuyOrSell curBOS = (BuyOrSell)curTrans;

                    BuyOrSell createdTuple = null;
                    BuyOrSell foundTuple = null;

                    foreach (BuyOrSell heldTuple in HeldStockList)
                    {
                        if (curBOS.StockName == heldTuple.StockName)
                        {
                            if (curBOS.BuyOrSellState == BuyOrSell.BuyOrSellEnum.Buy)
                            {
                                createdTuple = new BuyOrSell(heldTuple.StockName, (int)heldTuple.Quantity + curBOS.Quantity, heldTuple.PricePerStock, BuyOrSell.BuyOrSellEnum.Buy);

                            }
                            else
                            {
                                createdTuple = new BuyOrSell(heldTuple.StockName, (int)heldTuple.Quantity - curBOS.Quantity, heldTuple.PricePerStock, BuyOrSell.BuyOrSellEnum.Buy);

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
