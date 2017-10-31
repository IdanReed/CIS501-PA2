using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Controller: MVCEventSystem.Broadcaster<Error>
    {
        public const int HIGH_VOL_MAX = 16;
        public const int HIGH_VOL_MIN = 3;

        public const int MED_VOL_MAX = 9;
        public const int MED_VOL_MIN = 2;

        public const int LOW_VOL_MAX = 5;
        public const int LOW_VOL_MIN = 1;
        
        private MainModel _mainModel;
        private Portfolio _currentPortfolio;

        /// <summary>
        /// The main contructor for Controller
        /// </summary>
        /// <param name="m">The main model for the program</param>
        public Controller(MainModel m)
        {
            _mainModel = m;
        }
        #region AccountMethods
        /// <summary>
        /// Deposits money into the account
        /// </summary>
        /// <param name="e">The deposit event sent from the input view</param>
        /// <returns>Error.None always</returns>
        [EventListenerAttr("deposit")]
        private Error Deposit(DepositWithdrawEvent e)
        {
            _mainModel.Account.Deposit(e.Amount);
            Broadcast(new DisplayEvent("account"));
            return Error.None;
        }

        /// <summary>
        /// Withdraws money from the account
        /// </summary>
        /// <param name="e">The withdraw event sent from the input view</param>
        /// <returns>An error if not enough money. Otherwise Error.None</returns>
        [EventListenerAttr("withdraw")]
        private Error Withdraw(DepositWithdrawEvent e)
        {
            try
            {
                _mainModel.Account.Withdraw(e.Amount);
            }
            catch(ArgumentException err)
            {
                return new Error(err.Message);
            }
            Broadcast(new DisplayEvent("account"));
            return Error.None;
        }

        /// <summary>
        /// Simulates the stock prices
        /// </summary>
        /// <param name="e"></param>
        /// <returns>Error.None always</returns>
        [EventListenerAttr("simulate")]
        private Error Simulate(SimulateEvent e)
        {
            string vol = e.Volatility.ToUpper();

            int volMax = 0;
            int volMin = 0;
            switch (vol)
            {
                case "HIGH":
                    volMax = HIGH_VOL_MAX;
                    volMin = HIGH_VOL_MIN;
                    break;
                case "MED":
                    volMax = MED_VOL_MAX;
                    volMin = MED_VOL_MIN;
                    break;
                case "LOW":
                    volMax = LOW_VOL_MAX;
                    volMin = LOW_VOL_MIN;
                    break;
                default:
                    return new Error(e.Volatility + " is not a valid volatility level.");
            }

            Random r = new Random();
            foreach(Stock s in _mainModel.Stocks)
            {
                double selectedVol = r.Next(volMin, volMax)/100.0;
                int addSubtract = r.Next(2);

                double percOfStock = selectedVol * s.Price;
                if(addSubtract == 1) //Subtract
                {
                    s.Price -= percOfStock;
                }
                else
                {
                    s.Price += percOfStock;
                }
            }

            Broadcast(new DisplayEvent("account"));
            return Error.None;
        }
        #endregion AccountMethods

        #region PortfolioMethods

        /// <summary>
        /// Deletes the specified portfolio from the account
        /// </summary>
        /// <param name="e">The portfolio event containing the name of the portfolio to delete</param>
        /// <returns>An error if the portfolio doesn't exist, otherwise Error.None</returns>
        [EventListenerAttr("deletePortfolio")]
        private Error DeletePortfolio(PortfolioEvent e)
        {
            try
            {
                _mainModel.Account.DeletePortfolio(e.PortfolioName);
            }
            catch(ArgumentException err)
            {
                return new Error(err.Message);
            }
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }

        /// <summary>
        /// Creates a portfolio with the specified name
        /// </summary>
        /// <param name="e">The portfolio event containing the name of the portfolio to create</param>
        /// <returns>Error.None always</returns>
        [EventListenerAttr("newPortfolio")]
        private Error NewPortfolio(PortfolioEvent e)
        {
            _mainModel.Account.CreatePortfolio(e.PortfolioName, _mainModel.VerifyStock);
            _currentPortfolio = _mainModel.Account.GetPortfolioByName(e.PortfolioName);
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }

        /// <summary>
        /// Sets _currentPortfolio to the Portfolio with the specified name
        /// </summary>
        /// <param name="e">The portfolio event containing the name of the portfolio to set as current</param>
        /// <returns>An error if the portfolio doesn't exist, otherwise Error.None</returns>
        [EventListenerAttr("viewPortfolio")]
        private Error ViewPortfolio(PortfolioEvent e)
        {
            if(_mainModel.Account.Portfolios.Exists((p) => p.Name == e.PortfolioName))
            {
                _currentPortfolio = _mainModel.Account.GetPortfolioByName(e.PortfolioName);
            }
            else
            {
                return new Error("No portfolio exists with that name.");
            }
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }

        /// <summary>
        /// Sells the specified number of stocks from the current portfolio
        /// </summary>
        /// <param name="e">The stock event containing the name and number of stocks to sell</param>
        /// <returns>An error if there is no such stock, otherwise Error.None</returns>
        [EventListenerAttr("sellStocks")]
        private Error SellStocks(StockEvent e)
        {
            Stock s = _mainModel.Stocks.Find((stock) => stock.Name == e.Name);
            if(s != null)
            {
                _currentPortfolio.SellStock(s, e.Amount);
            }
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }

        /// <summary>
        /// Buys the specified number of stocks from the current portfolio
        /// </summary>
        /// <param name="e">The stock event containing the name and number of stocks to buy</param>
        /// <returns>An error if there is no such stock, otherwise Error.None</returns>
        [EventListenerAttr("buyStocks")]
        private Error BuyStocks(StockEvent e)
        {
            Stock s = _mainModel.Stocks.Find((stock) => stock.Name == e.Name);
            if(s != null)
            {
                try
                {
                    _currentPortfolio.PurchaseStock(s, e.Amount);
                }
                catch(ArgumentException err)
                {
                    return new Error(err.Message);
                }
            }
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }

        #endregion PortfolioMethods
    }
}
