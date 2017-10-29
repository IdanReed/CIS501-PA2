using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Stock
    {
        public readonly string name;
        public readonly string tag;
        public readonly double price;

        public Stock(string nameIn, string tagIn, double priceIn)
        {
            name = nameIn;
            tag = tagIn;
            price = priceIn;
        }
        public string ToString()
        {
            return name + "-" + tag + "-" + price;
        }
        public override bool Equals(Object obj)
        {
            Stock stock = obj as Stock;
            if (stock == null) return false;
            return name == stock.name && tag == stock.tag && price == stock.price;
        }

    }
}
