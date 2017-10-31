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

        public enum BuyOrSellEnum { Buy, Sell }; //declare BuyOrSellEnum

        /// <summary>
        /// Constructor for BuyOrSell object
        /// </summary>
        /// <param name="stockNameIn">Name of stock that is bought or sold</param>
        /// <param name="quantityIn">Nuber of shares bought</param>
        /// <param name="pricePerStockIn">Price per share</param>
        /// <param name="BORSIn">Whether the stock was bought or sold</param>
        public BuyOrSell(string stockNameIn, int quantityIn, double pricePerStockIn, BuyOrSellEnum BORSIn)
        {
            StockName = stockNameIn;
            Quantity = quantityIn;
            PricePerStock = pricePerStockIn;
            BuyOrSellState = BORSIn;
        }

        /// <summary>
        /// Constructor for BuyOrSell object
        /// </summary>
        /// <param name="stock">Stock object that is being bought or sold</param>
        /// <param name="quanittyIn">Number of shares bought</param>
        /// <param name="BORSIn">Whether the stock was bought or sold</param>
        public BuyOrSell(Stock stock, int quanittyIn, BuyOrSellEnum BORSIn)
        {
            StockName = stock.Name;
            PricePerStock = stock.Price;

            Quantity = quanittyIn;
            
            BuyOrSellState = BORSIn;
        }

        /// <summary>
        /// Calculates the gain or loss influence of the transaction. If the transaction was a purchase, the negation of the
        /// quantity multiplied by the price is returned and vice versa for a sell
        /// </summary>
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
