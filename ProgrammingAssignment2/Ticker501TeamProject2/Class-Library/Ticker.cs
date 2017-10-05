using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Ticker
    {
        private string _name;
        private string _tag;
        private double _price;

        public Ticker(string tag, string name, double price)
        {
            _tag = tag;
            _name = name;
            _price = price;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// UpdatePrice
        /// Gets an updated price from the simulation
        /// </summary>
        /// <param name="sim"></param>
        public void UpdatePrice(Simulation sim)
        {
            _price = sim.GetAdjustedPrice(_price);
        }
        
        
        public override string ToString()
        {
            return (_tag + " - " + _name + " - " + _price.ToString("C"));
        }


    }
}
