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

        public Stock(string nameIn, string tagIn, double priceIn)
        {
            Name = nameIn;
            Tag = tagIn;
            _price = priceIn;
        }
        public override string ToString()
        {
            return Name + "-" + Tag + "-" + _price;
        }
        public override bool Equals(Object obj)
        {
            Stock stock = obj as Stock;
            if (stock == null) return false;
            return Name == stock.Name && Tag == stock.Tag && _price == stock.Price;
        }

    }
}
