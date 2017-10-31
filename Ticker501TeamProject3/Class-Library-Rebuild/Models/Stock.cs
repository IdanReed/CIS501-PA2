using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Stock
    {
        public readonly string Name;
        public readonly string Tag;

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// Constructor for stock object
        /// </summary>
        /// <param name="nameIn">Name of stock (eg. Microsoft Inc.)</param>
        /// <param name="tagIn">Stock tage (eg. MSFT)</param>
        /// <param name="priceIn">Starting price of stock</param>
        public Stock(string nameIn, string tagIn, double priceIn)
        {
            Name = nameIn;
            Tag = tagIn;
            _price = priceIn;
        }

        /// <summary>
        /// Returns string value for displaying the stock, tag, and price
        /// </summary>
        /// <returns>String containing stock name, tag, and current price</returns>
        public override string ToString()
        {
            return Name + "-" + Tag + "-" + _price;
        }

        /// <summary>
        /// Checks to see if two stock objects are equal
        /// </summary>
        /// <param name="obj">Stock object to be checked</param>
        /// <returns>Boolean value of whether or not two stock objects are equal</returns>
        public override bool Equals(Object obj)
        {
            Stock stock = obj as Stock;
            if (stock == null) return false;
            return Name == stock.Name && Tag == stock.Tag && _price == stock.Price;
        }

    }
}
