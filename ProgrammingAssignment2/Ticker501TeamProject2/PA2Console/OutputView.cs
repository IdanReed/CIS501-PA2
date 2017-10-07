using Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    class OutputView
    {

        private Account _acct;
        private List<Ticker> _tickers;
        public OutputView(Account a, List<Ticker> tickers)
        {
            _acct = a;
            _tickers = tickers;
        }
        public void Update(Event e)
        {
            switch (e.Type)
            {
                //Respond to an event sent from Controller
                case "accountStats":
                    DisplayAccount();
                    break;
                case "accountBalance":
                    DisplayFunds();
                    break;
                case "showStocks":
                    ShowStocks();
                    break;
                case "showPortfolios":
                    ShowPortfolios();
                    break;
                case "portStats":
                    Portfolio p = e.Data as Portfolio;
                    ShowPortfolioStats(p);
                    break;
            }            
        }

        private void DisplayAccount()
        {
            //Do display stuff
            //Build amount
            Console.WriteLine("Funds: ${0}", _acct.Funds.ToString("N2"));
            MenuHelper.PressEnter();
        }

        private void DisplayFunds()
        {
            Console.WriteLine("Funds: ${0}", _acct.Funds.ToString("N2"));
        }

        private void ShowStocks()
        {
            Console.WriteLine("Stocks Available:");
            foreach(Ticker t in _tickers)
            {
                Console.WriteLine("\t{0}", t.ToString());
            }
        }
        private void ShowPortfolios()
        {
            Console.WriteLine("Here are your current portfolios");
            foreach (Portfolio p in _acct.Portfolios)
            {
                Console.WriteLine("\t- {0}", p.Name);
            }
        }

        private void ShowPortfolioStats(Portfolio p)
        {
            Console.WriteLine("Portfolio " + p.Name + ": ");
            if (p.Stocks.Count > 0)
            {
                foreach (StockPurchase s in p.Stocks)
                {
                    double percent = s.TotalPrice / p.CashValue;
                    double numPercent = s.Amount / (double)p.AmountStocks;

                    Console.WriteLine("\t" + s.TotalPrice.ToString("C") + "\t- Cash Value %: (" + String.Format("{0:P2}", percent) + ") - # Stocks (" + s.Amount + "): [" + String.Format("{0:P2}", numPercent) + "] - " + s.Ticker.Tag + " " + s.Ticker.Name);
                }
            }
            else
            {
                Console.WriteLine("You have no stocks to view");
            }
        }
    }
}
