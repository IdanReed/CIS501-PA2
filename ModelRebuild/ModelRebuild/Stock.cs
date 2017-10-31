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
        public readonly double Price;

        public Stock(string nameIn, string tagIn, double priceIn)
        {
            Name = nameIn;
            Tag = tagIn;
            Price = priceIn;
        }
        public override string ToString()
        {
            return Name + "-" + Tag + "-" + Price;
        }
        public override bool Equals(Object obj)
        {
            Stock stock = obj as Stock;
            if (stock == null) return false;
            return Name == stock.Name && Tag == stock.Tag && Price == stock.Price;
        }

    }
}
