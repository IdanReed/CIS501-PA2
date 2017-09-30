using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Stock
    {
        int amount;
        private double totalPrice;
        public Ticker ticker;
        private double initPrice;

        public Stock()
        {

        }

        public Stock(Ticker t, int a)
        {
            ticker = t;
            amount = a;
            totalPrice = ticker.Price * amount;
            initPrice = totalPrice;
        }

        public double InitPrice
        {
            get { return initPrice; }
        }

        public double TotalPrice
        {
            get {
                totalPrice = ticker.Price * amount;
                return ticker.Price * amount; }
        }
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public override string ToString()
        {
            totalPrice = ticker.Price * amount;
            return ticker.Tag + " - " + ticker.Name + " - " + totalPrice.ToString("C");
        }


    }
}
