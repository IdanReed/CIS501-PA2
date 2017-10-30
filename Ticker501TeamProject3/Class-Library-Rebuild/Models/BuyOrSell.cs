using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class BuyOrSell : Transaction
    {
        public readonly string StockName;
        public readonly int Quantity;
        public readonly double PricePerStock;
        public readonly BuyOrSellEnum BuyOrSellState;

        public enum BuyOrSellEnum { Buy, Sell };
        public BuyOrSell(string stockNameIn, int quantityIn, double pricePerStockIn, BuyOrSellEnum BORSIn)
        {
            StockName = stockNameIn;
            Quantity = quantityIn;
            PricePerStock = pricePerStockIn;
            BuyOrSellState = BORSIn;
        }

        public BuyOrSell(Stock stock, int quanittyIn, BuyOrSellEnum BORSIn)
        {
            StockName = stock.Name;
            PricePerStock = stock.Price;

            Quantity = quanittyIn;
            
            BuyOrSellState = BORSIn;
        }

        public double GainLossInfluence
        {
            get
            {
                double absValue = Quantity * PricePerStock;
                if (BuyOrSellState == BuyOrSellEnum.Buy)
                {
                    return absValue * -1;
                }
                else
                {
                    return absValue;
                }
            }

        }
    }
}
