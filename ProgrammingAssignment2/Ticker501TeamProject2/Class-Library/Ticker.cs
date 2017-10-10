/// <summary>
/// Ticker
/// Holds the name, tag, and price of the tickers read from the file
/// </summary>
/// <param name="price"></param>
/// <returns></returns>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Ticker
    {
        private string _name;//Full business name
        private string _tag;//Business ticker
        private double _price;//Price of the stock

        /// <summary>
        /// Ticker Constructor
        /// Constructs a new ticker with a tag, name, and price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public Ticker(string tag, string name, double price)
        {
            _tag = tag;
            _name = name;
            _price = price;
        }

        //Getters/Setters
        #region Getters/Setters
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
        #endregion


        /// <summary>
        /// UpdatePrice
        /// Gets an updated price from the simulation
        /// </summary>
        /// <param name="sim"></param>
        public void UpdatePrice(Simulation sim)
        {
            _price = sim.GetAdjustedPrice(_price);
        }

        /// <summary>
        /// ToString
        /// Returns the ticker in string format
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public override string ToString()
        {
            return (_tag + " - " + _name + " - " + _price.ToString("C"));
        }

        /// <summary>
        /// Equals
        /// returns whether two tickers equal each other
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool Equals(Ticker ticker)
        {
            return _name == ticker._name && _price == ticker._price && _tag == ticker._tag;
        }

    }
}
