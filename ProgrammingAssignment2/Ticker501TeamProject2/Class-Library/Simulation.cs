using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public class Simulation
    {
        string volatility; 
        
        public Simulation (string vol)
        {
            volatility = vol;
        }

        public string Volatility
        {
            get { return volatility; }
            set { volatility = value; }
        }

     
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
            if (volatility == "high")
            {
                percent = rand.Next(3, 16);
                percent /= 100;
            }
            else if (volatility == "med")
            {
                percent = rand.Next(2, 9);
                percent /= 100;
            }
            else if (volatility == "low")
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
