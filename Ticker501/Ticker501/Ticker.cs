///Jarod Honas
///501SF17
///A1
///Ticker
///Holds the name, tag, price for a company

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Ticker501
{
    

    class Ticker
    {
        string name;
        string tag;
        double price;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// UpdatePrice
        /// Gets an updated price from the simulation
        /// </summary>
        /// <param name="sim"></param>
        public void UpdatePrice(Simulation sim)
        {
            price = sim.GetAdjustedPrice(price);
        }
        
        
        public override string ToString()
        {
            return (tag + " - " + name + " - " + price.ToString("C"));
        }

    }
}
