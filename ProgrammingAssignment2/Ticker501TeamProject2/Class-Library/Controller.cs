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
    public delegate void Observer();

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
    }
    public class Controller
    {
        public const double TRADE_FEE = 9.99; //renamed these to be more clear about which is which
        public const double DEPOSIT_FEE = 4.99;
        public const int MAX_PORTFOLIOS = 3;

        private List<Observer> _registry;
        private Account _acct;
       
        public Controller(Account acct)
        {
            _registry = new List<Observer>();
            _acct = acct;
        }

        public void AddListener(Observer o)
        {
            _registry.Add(o);
        }
        public void Update()
        {
            foreach (Observer o in _registry)
            {
                o();
            }
        }


        #region Controlling Methods

        public Error InputHandle(Event e)
        {
            switch (e.Type)
            {
                case "deposit":
                    return Deposit((double) e.Data);
                case "withdraw":
                    return Withdraw((double) e.Data);
            }
            return Error.None;
        }

        #region Account Level
        private Error Deposit(double amount)
        {
            _acct.Funds += amount - DEPOSIT_FEE;
            _acct.DepositFees += DEPOSIT_FEE;
            //Do gains/losses stuff with the transfer fee
            return Error.None; 
        }
        private Error Withdraw(double amount)
        {
            if (_acct.Funds -  amount - DEPOSIT_FEE > 0)
            {
                _acct.Funds -= amount + DEPOSIT_FEE;
                _acct.DepositFees += DEPOSIT_FEE;
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

                return Error.None;
            }
            else
            {
                //Error saying too many portfolios
                return new Error("You have reached the maximum number of portfolios");
            }
        }
        #endregion Account Level

        #region Portfolio Level
        private Error PortView(Portfolio p)
        {
            if(p.Stocks.Count > 0)
            {
                foreach(StockPurchase s in p.Stocks)
                {
                    double percent = s.TotalPrice / p.CashValue;
                    double numPercent = s.Amount / (double)p.AmountStocks;
                    //Output the stuff
                }
                return Error.None;
            }
            else
            {
                return new Error("You have no stocks to view. Use 'buy' to start investing.");
            }
        }
        private Error PortBuy(Portfolio p, Ticker t, int amt)
        {
            double totalCost = t.Price * amt - TRADE_FEE;
            if (totalCost > _acct.Funds)
            {
                return new Error("You have inssuficient funds for this transaction.");
            }
            else
            {
                p.AmountStocks += amt;
                StockPurchase stock = new StockPurchase(t, amt);
                p.Stocks.Add(stock);
                _acct.Funds -= totalCost;
                //Say transfer has occured with output view

                return Error.None;
            }

        } 
        private Error PortSell(Portfolio p, Ticker t, double amt)
        {
            foreach(StockPurchase s in p.Stocks)
            {
                if(s.Ticker.Tag == t.Tag)
                {
                    _acct.Funds += s.TotalPrice - TRADE_FEE;
                    //Say transfer has occured
                    p.Stocks.Remove(s);
                    return Error.None;
                }
            }
            return new Error("You don't own any of that stock");
        }
        #endregion Portfolio Level

        #endregion Controlling Methods



        #region Getter/Setter

        public Account Account
        {
            get
            {
                return _acct;
            }
            set
            {
                _acct = value;
            }
        }



        #endregion Getter/Setter
    }
}
