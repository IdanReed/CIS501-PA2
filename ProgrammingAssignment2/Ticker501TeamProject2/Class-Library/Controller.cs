using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    public delegate void PortfolioTransactionHandler<T>(Portfolio p, Ticker t, T data);
    public delegate void PortfolioHandler<T>(Portfolio p);
    public delegate void InputHandler<T>(T data);
    public delegate void Observer();
    public delegate void ErrorHandler(Error e);

    public class Error
    {
        private string _message;
        //Any other potential stuff
        public string Message
        {
            get
            {
                return _message;
            }
        }

        public Error(string m)
        {
            _message = m;
        }
    }
    public class Controller
    {
        public const double TRADE_FEE = 9.99; //renamed these to be more clear about which is which
        public const double DEPOSIT_FEE = 4.99;
        public const int MAX_PORTFOLIOS = 3;

        private List<Observer> _registry;
        private Account _acct;
        private ErrorHandler _errorHandler;
       
        public Controller(ErrorHandler errHan)
        {
            _registry = new List<Observer>();
            _acct = new Account();
            _errorHandler = errHan;
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

        #region Account Level
        public void Deposit(double amount)
        {
            _acct.Funds += amount - DEPOSIT_FEE;
            _acct.DepositFees += DEPOSIT_FEE;
            //Do gains/losses stuff with the transfer fee

        }
        public void Withdraw(double amount)
        {
            if (_acct.Funds - amount - DEPOSIT_FEE > 0)
            {
                _acct.Funds -= amount + DEPOSIT_FEE;
                _acct.DepositFees += DEPOSIT_FEE;
                //Do gains/losses stuff with the tranfer fee
            }
            else
            {
                //Throw back to sell assets screen
            }
        }
        public void NewPortfolio(string name)
        {
            if (_acct.Portfolios.Count < MAX_PORTFOLIOS)
            {
                Portfolio newPortfolio = new Portfolio(name);

            }
            else
            {
                _errorHandler(new Error("You have reached the maximum number of portfolios."));
                //Error saying too many portfolios
            }
        }
        #endregion Account Level

        #region Portfolio Level
        public void PortView(Portfolio p)
        {
            if(p.Stocks.Count > 0)
            {
                foreach(StockPurchase s in p.Stocks)
                {
                    double percent = s.TotalPrice / p.CashValue;
                    double numPercent = s.Amount / (double)p.AmountStocks;
                    //Output the stuff
                }
            }
            else
            {
                _errorHandler(new Error("You have no stocks to view. Use 'buy' to start investing."));
            }
        }
        public void PortBuy(Portfolio p, Ticker t, int amt)
        {
            double totalCost = t.Price * amt - TRADE_FEE;
            if (totalCost > _acct.Funds)
            {
                _errorHandler(new Error("You have insufficient funds for this transaction."));
            }
            else
            {
                p.AmountStocks += amt;
                StockPurchase stock = new StockPurchase(t, amt);
                p.Stocks.Add(stock);
                _acct.Funds -= totalCost;
                //Say transfer has occured with output view
            }

        } 
        public void PortSell(Portfolio p, Ticker t, double amt)
        {
            foreach(StockPurchase s in p.Stocks)
            {
                if(s.Ticker.Tag == t.Tag)
                {
                    _acct.Funds += s.TotalPrice - TRADE_FEE;
                    //Say transfer has occured
                    p.Stocks.Remove(s);
                }
            } 
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

        public double TradeFee
        {
            get
            {
                return TRADE_FEE;
            }
        }

        public double DepositFee
        {
            get
            {
                return DEPOSIT_FEE;
            }
        }

        #endregion Getter/Setter
    }
}
