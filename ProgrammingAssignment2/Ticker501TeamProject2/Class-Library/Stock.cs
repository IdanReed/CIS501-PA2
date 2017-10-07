using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class StockPurchase
    {
        private int _amount;
        private double _totalPrice;
        private Ticker _ticker;
        private double _initPrice;

        public StockPurchase()
        {

        }

        public StockPurchase(Ticker t, int a)
        {
            _ticker = t;
            _amount = a;
            _totalPrice = _ticker.Price * _amount;
            _initPrice = _totalPrice;
        }

        public double InitPrice
        {
            get { return _initPrice; }
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
        public override string ToString()
        {
            _totalPrice = _ticker.Price * _amount;
            return _ticker.Tag + " - " + _ticker.Name + " - " + _totalPrice.ToString("C");
        }
        public bool HasSameTicker(StockPurchase SP)
        {
            return SP.Ticker.Equals(_ticker);
        }
        public StockPurchase Add(StockPurchase SP)
        {
            return new StockPurchase(_ticker, _amount + SP._amount);
        }

    }
}
