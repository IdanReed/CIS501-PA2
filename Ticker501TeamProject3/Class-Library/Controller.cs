using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
   // public delegate void PortfolioTransactionHandler<T>(Portfolio p, Ticker t, T data);
   // public delegate void PortfolioHandler<T>(Portfolio p);
    /// <summary>
    /// These two delegates are the main handles used by both input view and output view
    /// Input view gets controllers InputHandler and controller gets output views observer
    /// </summary>
    /// <param name="e">The event to pass into the input handler</param>
    /// <returns>An error if one occured</returns>
    public delegate Error InputHandler(Event e);
    public delegate void Observer(Event e);

    /// <summary>
    /// A simple error class that contains a message and a method to catch an error
    /// Has Error.None if no error occcured
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The error message
        /// </summary>
        private string _message;
        /// <summary>
        /// Tells if an error occured
        /// </summary>
        private bool _errorOccured;
        /// <summary>
        /// The default "No error occured"
        /// </summary>
        public static Error None = new Error();
        //Any other potential stuff
        /// <summary>
        /// Gets the _message variable
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
        }
        /// <summary>
        /// The default constructor for error where no error occured
        /// </summary>
        public Error()
        {
            _errorOccured = false;
            _message = "";
        }
        /// <summary>
        /// Construct an Error where an error occured and has a message
        /// </summary>
        /// <param name="m"></param>
        public Error(string m)
        {
            _errorOccured = true;
            _message = m;
        }
        /// <summary>
        /// Catches an error. If an error occured then it runs the Action a, otherwise it does nothing
        /// </summary>
        /// <param name="a">The action to run if an error occured</param>
        public void Catch(Action<string> a)
        {
            if (_errorOccured)
            {
                a(_message);
            }
        }
    }
    
    /// <summary>
    /// A basic event class which has a type and data associated with it
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        private string _type;
        /// <summary>
        /// The data stored along with the event
        /// </summary>
        private object _data;

        /// <summary>
        /// The getter for the event type
        /// </summary>
        public string Type
        {
            get { return _type; }
        }
        /// <summary>
        /// The getter for the event data
        /// </summary>
        public object Data
        {
            get { return _data; }
        }
        /// <summary>
        /// Creates an event with a type and with the specified data
        /// </summary>
        /// <param name="data">The data to store in the event</param>
        /// <param name="type">The type of the event</param>
        public Event(object data, string type)
        {
            _data = data;
            _type = type;
        }
        /// <summary>
        /// Creates an event with no data and a type
        /// </summary>
        /// <param name="type">The type of the evnet</param>
        public Event(string type)
        {
            _type = type;
            _data = null;
        }
    }
    /// <summary>
    /// The controller class does all of the computation and handles all the data validation
    /// between the input and output views.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The fee for Trading
        /// </summary>
        public const double TRADE_FEE = 9.99; //renamed these to be more clear about which is which
        /// <summary>
        /// The fee for depositing or withdrawing
        /// </summary>
        public const double DEPOSIT_FEE = 4.99;
        /// <summary>
        /// The maximum number of portfolios allowed
        /// </summary>
        public const int MAX_PORTFOLIOS = 3;

        /// <summary>
        /// The observer registry
        /// </summary>
        private List<Observer> _registry;
        /// <summary>
        /// The main account for the program
        /// </summary>
        private Account _acct;
        /// <summary>
        /// The current portfolio being edited. 
        /// </summary>
        private Portfolio _currentPortfolio;
        /// <summary>
        /// A list of all the stocks
        /// </summary>
        private List<Ticker> _tickers;
        /// <summary>
        /// The simulator used to change stock prices
        /// </summary>
        private Simulation _simulation;
       
        /// <summary>
        /// The constructor for controller
        /// </summary>
        /// <param name="acct">The account to associate with the controller</param>
        /// <param name="tickers">A list of stock tickers</param>
        /// <param name="sim">A simulation model</param>
        public Controller(Account acct, List<Ticker> tickers, Simulation sim)
        {
            _registry = new List<Observer>();
            _acct = acct;
            _tickers = tickers;
            _simulation = sim;
        }

        /// <summary>
        /// Adds an observer to the registry
        /// </summary>
        /// <param name="o">The observer to add to the registry</param>
        public void AddListener(Observer o)
        {
            _registry.Add(o);
        }
        /// <summary>
        /// Broadcasts an event to all observers
        /// </summary>
        /// <param name="e">The event to broadcast</param>
        private void Broadcast(Event e)
        {
            foreach(Observer o in _registry)
            {
                o(e);
            }
        }

        #region Controlling Methods

        /// <summary>
        /// The main inputHandle for the controller. An input view would access controller through this
        /// </summary>
        /// <param name="e">The event to pass into the handle</param>
        /// <returns>Returns an error if one occured</returns>
        public Error InputHandle(Event e)
        {
            switch (e.Type)
            {
                //Account Events
                case "deposit":
                    return Deposit((double) e.Data);
                case "withdraw":
                    return Withdraw((double) e.Data);
                case "accountStats":
                    Broadcast(new Event("accountStats"));
                    break;
                case "accountBalance": //need to fix this for InputView, not sure how
                    Broadcast(new Event("accountBalance"));
                    break;
                case "deleteAllPortfolios":
                    DeleteAllPortfolios();
                    break;

                //Portfolio Events
                case "deletePort":
                    Error deletePortErr = DeletePortfolio((string)e.Data);
                    Broadcast(new Event("deletePort"));
                    return deletePortErr;
                case "showPortfolios":
                    Broadcast(new Class_Library.Event("showPortfolios"));
                    break;
                case "newPort":
                    Error err = NewPortfolio((string)e.Data);
                    Broadcast(new Event("newPort"));
                    return err;
                case "portView":
                    return PortView((string)e.Data);
                case "portBuyShares":
                    Tuple<string, int> buyData = (Tuple<string, int>) e.Data;
                    return PortBuy(buyData.Item1, buyData.Item2);
                case "portBuyCost":
                    Tuple<string, double> buyDataCost = (Tuple<string, double>)e.Data;
                    return PortBuy(buyDataCost.Item1, buyDataCost.Item2);
                case "portSell":
                    Tuple<string, int> sellData = (Tuple<string, int>)e.Data;
                    return PortSell(sellData.Item1, sellData.Item2);
                case "portStats":
                    Broadcast(new Event(_currentPortfolio, "portStats"));
                    break;
                case "showPortStocks":
                    Broadcast(new Event(_currentPortfolio, "showPortStocks"));
                    break;

                //Ticker Events
                case "simulate":
                    return Simulate((string)e.Data);                   
                case "showStocks":
                    Broadcast(new Event("showStocks"));
                    break;
          
            }
            //Broadcast(new Event(""));
            return Error.None;
        }

        /// <summary>
        /// Deletes all the portfolios
        /// </summary>
        /// <returns>Error.None is always returned</returns>
        private Error DeleteAllPortfolios()
        {
            while (_acct.Portfolios.Count > 0)
            {
                Portfolio p = _acct.Portfolios[0];
                DeletePortfolio(p.Name);
            }
            return Error.None;
        }

        #region Account Level
        /// <summary>
        /// Deposits the specified amount into the account
        /// </summary>
        /// <param name="amount">The amount to deposit</param>
        /// <returns>An error if the deposit amount is less than the Deposit_fee, otherwise Error.None</returns>
        private Error Deposit(double amount)
        {
            if (amount < DEPOSIT_FEE) return new Error("Not depositing enough to cover the deposit fee");

            _acct.Funds += amount - DEPOSIT_FEE;
            _acct.DepositFees += DEPOSIT_FEE;
            _acct.CurValue += amount;

            //Broadcast(new Event("depositWithdraw"));
            //Do gains/losses stuff with the transfer fee
            return Error.None; 
        }
        /// <summary>
        /// Withdraws the specified amount from the account
        /// </summary>
        /// <param name="amount">The amount to withdraw</param>
        /// <returns>An error if withdrawing too much money, otherwise Error.None</returns>
        private Error Withdraw(double amount)
        {
            if (_acct.Funds -  amount - DEPOSIT_FEE > 0)
            {
                _acct.Funds -= amount + DEPOSIT_FEE;
                _acct.DepositFees += DEPOSIT_FEE;
                //_acct.CurValue -= amount;
                _acct.CurValue = _acct.CurValue - amount;
                //Broadcast(new Event("depositWithdraw"));
                //Do gains/losses stuff with the tranfer fee
                return Error.None;
            }
            else
            {
                //Throw back to sell assets screen
                return new Error("Withdrawing too much money");
            }
        }
        /// <summary>
        /// Creates a portfolio with the specified name
        /// </summary>
        /// <param name="name">The name of the new portfolio</param>
        /// <returns>An error if max # portfolios is reached or name is "", otherwise Error.None</returns>
        private Error NewPortfolio(string name)
        {
            if (name.Length == 0) return new Error("Please enter a name for the new portfolio");
            if (_acct.Portfolios.Count < MAX_PORTFOLIOS)
            {
                Portfolio newPortfolio = new Portfolio(name);
                _acct.Portfolios.Add(newPortfolio);
                return Error.None;
            }
            else
            {
                //Error saying too many portfolios
                return new Error("You have reached the maximum number of portfolios");
            }
        }

        /// <summary>
        /// Deletes a portfolio with the specified name
        /// </summary>
        /// <param name="name">The name of the portfolio to delete</param>
        /// <returns>An error if no portfolios have been created or if the portfolio doesn't exist. Otherwise Error.None</returns>
        private Error DeletePortfolio(string name)
        {
            if (_acct.Portfolios.Count == 0) return new Error("You haven't created any portfolio's yet");
            
            Portfolio p = _acct.Portfolios.Find(port => port.Name == name);

            if (p == null) return new Error("A portfolio with that name does not exist.");
            double totalWorth = 0;
            foreach (StockPurchase s in p.Stocks)
            {
                totalWorth += s.TotalPrice;
            }
            _acct.Funds += totalWorth - TRADE_FEE;
            _acct.DepositFees -= p.ChangeInValue - TRADE_FEE;
            Broadcast(new Event("accountBalance"));
            _acct.Portfolios.Remove(p);
            return Error.None;
        }

      
        #endregion Account Level

        #region Portfolio Level
        /// <summary>
        /// Sets _currentPortfolio to the portfolio with the specified name
        /// </summary>
        /// <param name="name">The name of the portfolio to set as current</param>
        /// <returns>An error if no portfolios or portfolio with name doesn't exist. Otherwise Error.None</returns>
        private Error PortView(string name)
        {
            if (_acct.Portfolios.Count == 0) return new Error("You have not created any portfolios yet.");
            Portfolio p = _acct.Portfolios.Find(port => port.Name == name);
            if(p != null)
            {
                _currentPortfolio = p;
                return Error.None;
            }
            else
            {
                return new Error("A portfolio with that name does not exist");
            }
        }
       
        /// <summary>
        /// Buys a specified amount of stock of a specified stock
        /// </summary>
        /// <param name="tickerName">The abbreviation of the stock to buy</param>
        /// <param name="amt">The amount to buy</param>
        /// <returns>Error if trying to buy less than 1 share, Error if stock doesnt exist, Error if insufficient funds, Error if already own stocks in company. </returns>
        private Error PortBuy(string tickerName, int amt)
        {
            if (amt <= 0) return new Error("Cannot buy less than 1 share");
            Ticker t = GetTickerByAbbr(tickerName.ToUpper());

            if (t == null) return new Error("A stock with that abbreviation does not exist.");
    
            double totalCost = (t.Price * amt) + TRADE_FEE;
            if (totalCost > _acct.Funds)
            {
                return new Error("You have inssuficient funds for this transaction.");
            }
            else
            {
                if(_currentPortfolio.Stocks.Exists(pur => pur.Ticker.Tag == tickerName))
                {
                    return new Class_Library.Error("You cannot buy stock from a company you already have stock in: ");
                }
                _currentPortfolio.AmountStocks += amt;
                StockPurchase stock = new StockPurchase(t, amt);
                _currentPortfolio.Stocks.Add(stock);
                _acct.Funds -= totalCost;
                _currentPortfolio.TotalFees += TRADE_FEE;
                return Error.None;
            }

        } 
        /// <summary>
        /// Buys a specified amount worth of stock of a specified stock. Calls PortBuy(string, int);
        /// </summary>
        /// <param name="tickerName">The abbreviation of the stock to buy</param>
        /// <param name="cost">The amount in dollars of stock you'd like to buy</param>
        /// <returns>An error if the stock does not exist</returns>
        /// <seealso cref="PortBuy(string, int)"/>
        private Error PortBuy(string tickerName, double cost)
        {
            Ticker t = GetTickerByAbbr(tickerName);

            if (t == null) return new Class_Library.Error("A stock with that abbreviation does not exist.");

            int amount = (int)(cost / t.Price);
            _currentPortfolio.TotalFees += TRADE_FEE;
            return PortBuy(tickerName, amount);
        }
        /// <summary>
        /// Sells a specified amount of shares of a specified stock
        /// </summary>
        /// <param name="tickerName">The abbreviation of the stock to sell</param>
        /// <param name="amt">The amount of shares to sell</param>
        /// <returns>An error if stock doesn't exist. An error if you don't own any of the stock. An error if trying to sell more than you own</returns>
        private Error PortSell(string tickerName, int amt)
        {
            Ticker t = GetTickerByAbbr(tickerName.ToUpper());
            if (t == null) return new Error("A stock with that abbreviation does not exist.");

            foreach(StockPurchase s in _currentPortfolio.Stocks)
            {
                if(s.Ticker.Tag == t.Tag)
                {

                    //Say transfer has occured
                    if (amt > s.Amount) return new Error("You don't have that many stocks to sell");
                    else if(amt == s.Amount)
                    {
                        _currentPortfolio.Stocks.Remove(s);
                        _acct.Funds += s.Ticker.Price * amt - TRADE_FEE;
                        _currentPortfolio.TotalFees -= s.TotalPrice - s.InitPrice;
                    }
                    else
                    {
                        _currentPortfolio.TotalFees -= s.Ticker.Price * amt - (s.InitPrice / s.Amount) * amt;
                        s.InitPrice = s.InitPrice / s.Amount * (s.Amount - amt);
                        s.Amount -= amt;
                        _acct.Funds += s.Ticker.Price * amt - TRADE_FEE;
 
                    }


                    _currentPortfolio.TotalFees += TRADE_FEE;
                    return Error.None;
                }
            }
            return new Error("You don't own any of that stock");
        }
        #endregion Portfolio Level

        #region Simulation Level

        /// <summary>
        /// Handles the simulation of the prices
        /// </summary>
        /// <param name="volatilityLevel">The volatility level you'd like to simulate</param>
        /// <returns>An error if "high" "med" or "low" is not chosen.</returns>
        private Error Simulate(string volatilityLevel)
        {
            string val = volatilityLevel.ToLower();
            if(val == "high" || val =="med" || val == "low")
            {
                _simulation.Volatility = val;
            }
            else
            {
                return new Class_Library.Error("Please choose either 'high', 'med', or 'low'");
            }

            foreach(Ticker t in _tickers)
            {
                t.UpdatePrice(_simulation);
            }
            Broadcast(new Event("showStocks"));
            return Error.None;
        }

        #endregion Simulation Level

        #endregion Controlling Methods

        #region Utils
        /// <summary>
        /// Gets a ticker by the abbreviation
        /// </summary>
        /// <param name="abbr">The abbreviation of the ticker</param>
        /// <returns>The ticker with the specified abbreviation</returns>
        private Ticker GetTickerByAbbr(string abbr)
        {
            return _tickers.Find(tic => tic.Tag == abbr);
        }

        #endregion Utils
    }
}
