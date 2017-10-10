/// <summary>
/// Simulation
/// Returns a new price for a stock based on a volatility
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
    public class Simulation
    {
        private string _volatility; //Volatility of the market


        
        /// <summary>
        /// Simulation Constructor
        /// gets the volatility for the simulation
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public Simulation (string vol)
        {
            _volatility = vol;
        }

        /*
         * Getters/Setters
         */
        #region Getters/Setters
        public string Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }
        #endregion

        /// <summary>
        /// GetAdjustedPrice
        /// returns a new price that varies depending on the current volatility
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public double GetAdjustedPrice(double price)
        {
            Random rand = new Random();
            double percent = 0;
            if (_volatility == "high")
            {
                percent = rand.Next(3, 16);
                percent /= 100;
            }
            else if (_volatility == "med")
            {
                percent = rand.Next(2, 9);
                percent /= 100;
            }
            else if (_volatility == "low")
            {
                percent = rand.Next(1, 5);
                percent /= 100;
            }
            int pos = rand.Next(1,11);
            if(pos > 5)
            {
                pos = 1;
            }
            else
            {
                pos = -1;
            }
            return price + ((price*percent)*pos);
        }//getadjustedprice


    }
}
