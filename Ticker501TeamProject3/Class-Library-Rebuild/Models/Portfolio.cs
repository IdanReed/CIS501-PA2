using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public delegate bool StockVerifier(Stock_M stock);
    public delegate bool FundsVerifier(BuyOrSell_M.BuyOrSellEnum type, double cost, double fee);
    /// <summary>
    /// Portfolio holds the name of itself and the stocks that it currently holds
    /// It can return its current value and purchase or sell stocks
    /// </summary>
    public class Portfolio_M
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
        public Portfolio_M(string nameIn, StockVerifier verifierStock, FundsVerifier fundsManager)
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
        public double HeldValueCurrent(List<Stock_M> stockList)
        {
            List<BuyOrSell_M> heldList = CurrentlyHeld();
            double sum = 0;
            foreach(BuyOrSell_M curBOS in heldList)
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
            List<BuyOrSell_M> historicHeld = CurrentlyHeld();

            double sum = 0;
            foreach (BuyOrSell_M curBOS in historicHeld)
            {
                sum += curBOS.Quantity * curBOS.PricePerStock;
            }
            return sum;
        }

        /// <summary>
        /// Returns the List of stocks held in the portfolio
        /// </summary>
        /// <returns></returns>
        public List<BuyOrSell_M> CurrentlyHeld()
        {
            List<BuyOrSell_M> HeldStockList = new List<BuyOrSell_M>();

            //Dictionary<BuyOrSell, int> amounts
            foreach (Transaction curTrans in _transactionList)
            {

                if (curTrans.GetType() == typeof(BuyOrSell_M))
                {
                   BuyOrSell_M curBOS = (BuyOrSell_M)curTrans;
                    
                    BuyOrSell_M found = HeldStockList.Find(bos => curBOS.StockName == bos.StockName);

                    if(found == null)
                    {
                        HeldStockList.Add(curBOS);
                    }else
                    {
                        if(curBOS.BuyOrSellState == BuyOrSell_M.BuyOrSellEnum.Buy)
                        {
                            HeldStockList[HeldStockList.IndexOf(found)] = new BuyOrSell_M(curBOS.StockName,curBOS.Quantity+found.Quantity, curBOS.PricePerStock, BuyOrSell_M.BuyOrSellEnum.Buy);
                        }else
                        {
                            if(curBOS.Quantity - found.Quantity != 0)
                            {
                                HeldStockList[HeldStockList.IndexOf(found)] = new BuyOrSell_M(curBOS.StockName, found.Quantity - curBOS.Quantity, curBOS.PricePerStock, BuyOrSell_M.BuyOrSellEnum.Buy);
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
        public void PurchaseStock(Stock_M stock, int amount)
        {
            if (_verifierStock(stock))
            {
                if(_fundsManager(BuyOrSell_M.BuyOrSellEnum.Buy, (stock.Price * amount), Fee_M.BUY_OR_SELL))
                {
                    BuyOrSell_M buyOrSell = new BuyOrSell_M(stock, amount, BuyOrSell_M.BuyOrSellEnum.Buy);
                    _transactionList.Add(buyOrSell);
                    Fee_M fee = new Fee_M(Fee_M.FeeSelect.BuyOrSell);
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
        public void SellStock(Stock_M stock, int amount)
        {
            if (_verifierStock(stock))
            {
                if (_fundsManager(BuyOrSell_M.BuyOrSellEnum.Sell, (stock.Price * amount), Fee_M.BUY_OR_SELL))
                {
                    BuyOrSell_M buyOrSell = new BuyOrSell_M(stock, amount, BuyOrSell_M.BuyOrSellEnum.Sell);
                    _transactionList.Add(buyOrSell);
                    Fee_M fee = new Fee_M(Fee_M.FeeSelect.BuyOrSell);
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
