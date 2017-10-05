using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
   // public delegate void PortfolioTransactionHandler<T>(Portfolio p, Ticker t, T data);
   // public delegate void PortfolioHandler<T>(Portfolio p);
    public delegate Error InputHandler(Event e);
    public delegate void Observer(Event e);

    public class Error
    {
        private string _message;
        private bool _errorOccured;
        public static Error None = new Error();
        //Any other potential stuff
        public string Message
        {
            get
            {
                return _message;
            }
        }

        public Error()
        {
            _errorOccured = false;
            _message = "";
        }
        public Error(string m)
        {
            _errorOccured = true;
            _message = m;
        }
        public void Catch(Action<string> a)
        {
            if (_errorOccured)
            {
                a(_message);
            }
        }
    }
    
    public class Event
    {
        private string _type;
        private object _data;

        public string Type
        {
            get { return _type; }
        }
        public object Data
        {
            get { return _data; }
        }
        public Event(object data, string type)
        {
            _data = data;
            _type = type;
        }
        public Event(string type)
        {
            _type = type;
            _data = null;
        }
    }
    public class Controller
    {
        public const double TRADE_FEE = 9.99; //renamed these to be more clear about which is which
        public const double DEPOSIT_FEE = 4.99;
        public const int MAX_PORTFOLIOS = 3;

        private List<Observer> _registry;
        private Account _acct;
        private Portfolio _currentPortfolio;
        private List<Ticker> _tickers;
        private Simulation _simulation;
       
        public Controller(Account acct, List<Ticker> tickers, Simulation sim)
        {
            _registry = new List<Observer>();
            _acct = acct;
            _tickers = tickers;
            _simulation = sim;
        }

        public void AddListener(Observer o)
        {
            _registry.Add(o);
        }
        //Broadcast an event to all observers
        private void Broadcast(Event e)
        {
            foreach(Observer o in _registry)
            {
                o(e);
            }
        }

        #region Controlling Methods

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

                //Portfolio Events
                case "deletePort":
                    return DeletePortfolio((string)e.Data);
                case "newPort":
                    return NewPortfolio((string)e.Data);
                case "portView":
                    return PortView((string)e.Data);
                case "portBuy":
                    Tuple<string, int> buyData = (Tuple<string, int>) e.Data;
                    return PortBuy(buyData.Item1, buyData.Item2);
                case "portSell":
                    Tuple<string, int> sellData = (Tuple<string, int>)e.Data;
                    return PortSell(sellData.Item1, sellData.Item2);
                case "portStats":
                    Broadcast(new Event("portStats"));
                    break;

                //Ticker Events
                case "simulate":
                    return Simulate((string)e.Data);                   
                case "showStocks":
                    Broadcast(new Event("showStocks"));
                    break;
          
            }
            return Error.None;
        }

        #region Account Level
        private Error Deposit(double amount)
        {
            _acct.Funds += amount - DEPOSIT_FEE;
            _acct.DepositFees += DEPOSIT_FEE;
            _acct.CurValue += amount;
            //Do gains/losses stuff with the transfer fee
            return Error.None; 
        }
        private Error Withdraw(double amount)
        {
            if (_acct.Funds -  amount - DEPOSIT_FEE > 0)
            {
                _acct.Funds -= amount + DEPOSIT_FEE;
                _acct.DepositFees += DEPOSIT_FEE;
                _acct.CurValue -= amount;
                //Do gains/losses stuff with the tranfer fee
                return Error.None;
            }
            else
            {
                //Throw back to sell assets screen
                return new Error("Withdrawing too much money");
            }
        }
        private Error NewPortfolio(string name)
        {
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

        private Error DeletePortfolio(string name)
        {
            if (_acct.Portfolios.Count == 0) return new Error("You haven't created any portfolio's yet");
            
            Portfolio p = _acct.Portfolios.Find(port => port.Name == name);

            if (p == null) return new Error("A portfolio with that name does not exsist.");
            double totalWorth = 0;
            foreach (StockPurchase s in p.Stocks)
            {
                totalWorth += s.TotalPrice;
            }
            _acct.Funds += totalWorth;
            Broadcast(new Event("accountBalance"));
            _acct.Portfolios.Remove(p);
            return Error.None;
        }

      
        #endregion Account Level

        #region Portfolio Level
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
                return new Error("A portfolio with that name does not exsist");
            }
        }
       
        private Error PortBuy(string tickerName, int amt)
        {
            Ticker t = GetTickerByAbbr(tickerName);

            if (t == null) return new Error("A stock with that abbreviation does not exsist.");
    
            double totalCost = t.Price * amt - TRADE_FEE;
            if (totalCost > _acct.Funds)
            {
                return new Error("You have inssuficient funds for this transaction.");
            }
            else
            {
                _currentPortfolio.AmountStocks += amt;
                StockPurchase stock = new StockPurchase(t, amt);
                _currentPortfolio.Stocks.Add(stock);
                _acct.Funds -= totalCost;

                return Error.None;
            }

        } 
        private Error PortSell(string tickerName, double amt)
        {
            Ticker t = GetTickerByAbbr(tickerName);
            if (t == null) return new Error("A stock with that abbreviation does not exsist.");

            foreach(StockPurchase s in _currentPortfolio.Stocks)
            {
                if(s.Ticker.Tag == t.Tag)
                {
                    _acct.Funds += s.TotalPrice - TRADE_FEE;
                    //Say transfer has occured
                    _currentPortfolio.Stocks.Remove(s);
                    return Error.None;
                }
            }
            return new Error("You don't own any of that stock");
        }
        #endregion Portfolio Level

        #region Simulation Level

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

            foreach(Ticker t in _acct.Tickers)
            {
                t.UpdatePrice(_simulation);
            }
            Broadcast(new Event("showStocks"));
            return Error.None;
        }

        #endregion Simulation Level

        #endregion Controlling Methods

        #region Utils

        private Ticker GetTickerByAbbr(string abbr)
        {
            return _tickers.Find(tic => tic.Tag == abbr);
        }

        #endregion Utils
    }
}
