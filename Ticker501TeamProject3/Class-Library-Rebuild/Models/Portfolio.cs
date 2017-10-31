using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public delegate bool StockVerifier(Stock stock);
    public delegate bool FundsVerifier(BuyOrSell.BuyOrSellEnum type, double cost, double fee);
    /// <summary>
    /// Portfolio holds the name of itself and the stocks that it currently holds
    /// It can return its current value and purchase or sell stocks
    /// </summary>
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
        /// <summary>
        /// Constructs a Portfolio with a name, verifierStock, and fundsManager
        /// </summary>
        /// <param name="nameIn"></param>
        /// <param name="verifierStock"></param>
        /// <param name="fundsManager"></param>
        public Portfolio(string nameIn, StockVerifier verifierStock, FundsVerifier fundsManager)
        {
            Name = nameIn;
            _verifierStock = verifierStock;
            _fundsManager = fundsManager;
        }

        /// <summary>
        /// Returns the Gains/Losses of the Portfolio
        /// </summary>
        /// <param name="stockList"></param>
        /// <returns></returns>
        public double GainLoss()
        {
            double gainLossSum = 0;
            foreach (Transaction transaction in _transactionList)
            {
                gainLossSum += transaction.GainLossInfluence;
            }

            gainLossSum += HeldValueAtPurchase();

            return gainLossSum;
        }

        /// <summary>
        /// Returns the current value of the Portfolio (The combined value of all the stocks)
        /// </summary>
        /// <param name="stockList"></param>
        /// <returns></returns>
        public double HeldValueCurrent(List<Stock> stockList)
        {
            List<BuyOrSell> heldList = CurrentlyHeld();
            double sum = 0;
            foreach(BuyOrSell curBOS in heldList)
            {

                double price = stockList.Find((s) => curBOS.StockName == s.Name).Price;
                sum += price * curBOS.Quantity;
                      
            }
            return sum;
        }

        /// <summary>
        /// Returns the value of the portfolio with the stocks evaluated at their initial price
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the List of stocks held in the portfolio
        /// </summary>
        /// <returns></returns>
        public List<BuyOrSell> CurrentlyHeld()
        {
            List<BuyOrSell> HeldStockList = new List<BuyOrSell>();

            //Dictionary<BuyOrSell, int> amounts
            foreach (Transaction curTrans in _transactionList)
            {

                if (curTrans.GetType() == typeof(BuyOrSell))
                {
                   BuyOrSell curBOS = (BuyOrSell)curTrans;
                    
                    BuyOrSell found = HeldStockList.Find(bos => curBOS.StockName == bos.StockName);

                    if(found == null)
                    {
                        HeldStockList.Add(curBOS);
                    }else
                    {
                        if(curBOS.BuyOrSellState == BuyOrSell.BuyOrSellEnum.Buy)
                        {
                            HeldStockList[HeldStockList.IndexOf(found)] = new BuyOrSell(curBOS.StockName,curBOS.Quantity+found.Quantity, curBOS.PricePerStock, BuyOrSell.BuyOrSellEnum.Buy);
                        }else
                        {
                            if(curBOS.Quantity - found.Quantity != 0)
                            {
                                HeldStockList[HeldStockList.IndexOf(found)] = new BuyOrSell(curBOS.StockName, found.Quantity - curBOS.Quantity, curBOS.PricePerStock, BuyOrSell.BuyOrSellEnum.Buy);
                            }else
                            {
                                HeldStockList.Remove(found);
                            }
                            
                        }
                    }
                 
                    

                }
            }
            return HeldStockList;
        }
       
        /// <summary>
        /// Purchases a new stock with a given amount and adds the stock to the portfolio
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="amount"></param>
        public void PurchaseStock(Stock stock, int amount)
        {
            if (_verifierStock(stock))
            {
                if(_fundsManager(BuyOrSell.BuyOrSellEnum.Buy, (stock.Price * amount), Fee.BUY_OR_SELL))
                {
                    BuyOrSell buyOrSell = new BuyOrSell(stock, amount, BuyOrSell.BuyOrSellEnum.Buy);
                    _transactionList.Add(buyOrSell);
                    Fee fee = new Fee(Fee.FeeSelect.BuyOrSell);
                    _transactionList.Add(fee);
                }
                else
                {
                    throw new ArgumentException("Not enough cash");
                }
            }
        }

        /// <summary>
        /// Sells the amount given of a stock held in the portfolio and sends the cash back to the account funds
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="amount"></param>
        public void SellStock(Stock stock, int amount)
        {
            if (_verifierStock(stock))
            {
                if (_fundsManager(BuyOrSell.BuyOrSellEnum.Sell, (stock.Price * amount), Fee.BUY_OR_SELL))
                {
                    BuyOrSell buyOrSell = new BuyOrSell(stock, amount, BuyOrSell.BuyOrSellEnum.Sell);
                    _transactionList.Add(buyOrSell);
                    Fee fee = new Fee(Fee.FeeSelect.BuyOrSell);
                    _transactionList.Add(fee);
                }
                else
                {
                    throw new ArgumentException("Not enough cash");
                }
            }
        }
    }
}
