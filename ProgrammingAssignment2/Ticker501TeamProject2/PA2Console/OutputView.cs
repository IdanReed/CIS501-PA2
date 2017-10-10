using Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    /// <summary>
    /// This is the class that handles events for output to the console. Method calls are passed to it from the 
    /// Controller class.
    /// </summary>
    class OutputView
    {

        private Account _acct; //account used to build values
        private List<Ticker> _tickers; //list of all tickers that are available to trade

        /// <summary>
        /// Constructor for the OutputView class
        /// </summary>
        /// <param name="a">Account that will be used</param>
        /// <param name="tickers">List of all tickers in the program at time of initialization</param>
        public OutputView(Account a, List<Ticker> tickers)
        {
            _acct = a;
            _tickers = tickers;
        }

        /// <summary>
        /// This is the main function in the class. It receives an event from the Controller class when something
        /// is broadcast. It then runs the appropriate function to display the needed information
        /// </summary>
        /// <param name="e">Event that is to run</param>
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
                case "showPortStocks":
                    Portfolio port2 = e.Data as Portfolio;
                    ShowPortStocks(port2);
                    break;
            }            
        }

        /// <summary>
        /// This method displays the money you have in your account, as well as the gain/losses for the entire account
        /// </summary>
        private void DisplayAccount()
        {
            //Build amount
            Console.WriteLine("Funds: ${0}", _acct.Funds.ToString("N2"));
            //Build Account level gain/loss
            Console.Write("Account Gain/Loss: ");
            if (_acct.ChangeInFunds < 0)
            {
                Console.Write("-");
            }
            Console.WriteLine("{0}", _acct.ChangeInFunds.ToString("C"));
            MenuHelper.PressEnter();
        }

        /// <summary>
        /// This method displays only the funds that are currently in the account
        /// </summary>
        private void DisplayFunds()
        {
            Console.WriteLine("Funds: ${0}", _acct.Funds.ToString("N2"));
        }

        /// <summary>
        /// This method shows all stocks that are available to trade
        /// </summary>
        private void ShowStocks()
        {
            Console.WriteLine("Stocks Available:");
            foreach(Ticker t in _tickers)
            {
                Console.WriteLine("\t{0}", t.ToString());
            }
        }

        /// <summary>
        /// This method shows the stocks that have been purchased by the user for a specified portfolio.
        /// </summary>
        /// <param name="p">The portfolio to show the stocks in</param>
        private void ShowPortStocks(Portfolio p)
        {
            foreach (StockPurchase s in p.Stocks)
            {
                Console.WriteLine(s.ToString());
            }
        }

        /// <summary>
        /// Shows the list of all portfolios that have been created
        /// </summary>
        private void ShowPortfolios()
        {
            Console.WriteLine("Here are your current portfolios");
            foreach (Portfolio p in _acct.Portfolios)
            {
                Console.WriteLine("\t- {0}", p.Name);
            }
        }

        /// <summary>
        /// Method that shows the stats of a desired portfolio
        /// </summary>
        /// <param name="p">The portfolio to show the stats for</param>
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
            Console.WriteLine("Gains\\Losses ${0}", p.ChangeInValue.ToString("N2"));
        }
    }
}
