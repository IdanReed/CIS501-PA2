using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Account
    {
        public double funds = 0;
        public double depositedAmount = 0;
        public double withdrawlAmount = 0;

        public List<Portfolio> portfolios = new List<Portfolio>();
        
        public double CalculateValue(List<Stock> stockList)
        {
            double sum = 0;
            foreach (Portfolio portfolio in portfolios)
            {
                sum += portfolio.Value(stockList);
            }
            sum += funds;
            return sum;
        }
        public double CalculateGainLoss(List<Stock> stockList)
        {
            double sum = 0;
            foreach (Portfolio portfolio in portfolios)
            {
                sum += portfolio.GainLoss(stockList);
            }
            return sum;
        }
        public void DeletePortfolio(int portNum)
        {
            if(portNum < portfolios.Count)
            {
                portfolios[portNum] = null;
            }
        }
        public bool CreatePortfolio(string name)
        {
            if (portfolios.Count > 2)
            {
                return false;
            }
            else
            {
                portfolios.Add(new Portfolio(name));
                return true;
            }

        }
    }
}
