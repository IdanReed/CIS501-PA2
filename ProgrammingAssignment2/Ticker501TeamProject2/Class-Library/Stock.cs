/// <summary>
/// StockPurchase
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
    public class StockPurchase
    {
        private int _amount;//amount of stocks held
        private double _totalPrice;//total pruce of all the stocks
        private Ticker _ticker;//the ticker associated with the stock
        private double _initPrice;//the initial price the stock was bought for

        public StockPurchase()
        {

        }

        
        /// <summary>
        /// StockPurchase Constructor
        /// Sets the ticker and the amount of stock purchased
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public StockPurchase(Ticker t, int a)
        {
            _ticker = t;
            _amount = a;
            _totalPrice = _ticker.Price * _amount;
            _initPrice = _totalPrice;
        }

        //Getters/Setters
        #region Getters/Setters
        public double InitPrice
        {
            get { return _initPrice; }
            set { _initPrice = value; }
        }

        public double TotalPrice
        {
            get {
                _totalPrice = _ticker.Price * _amount;
                return _ticker.Price * _amount; }
        }
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public Ticker Ticker
        {
            get
            {
                return _ticker;
            }
        }
        #endregion
        public override string ToString()
        {
            _totalPrice = _ticker.Price * _amount;
            return _ticker.Tag + " - " + _ticker.Name + " - " + _totalPrice.ToString("C");
        }


        /// <summary>
        /// HasSameTicker
        /// returns whether the StockPurchase passed has the same ticker as the instance
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool HasSameTicker(StockPurchase SP)
        {
            return SP.Ticker.Equals(_ticker);
        }

        /// <summary>
        /// Add
        /// Returns the new stock with the amount of stocks purchased added together
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public StockPurchase Add(StockPurchase SP)
        {
            return new StockPurchase(_ticker, _amount + SP._amount);
        }

    }
}
