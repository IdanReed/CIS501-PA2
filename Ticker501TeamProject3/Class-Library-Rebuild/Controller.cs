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
        private MainModel _mainModel;
        private Portfolio _currentPortfolio;
        public Controller(MainModel m)
        {
            _mainModel = m;
        }
        /*
         * case "deposit":
                    return Deposit((double) e.Data);
                case "withdraw":
                    return Withdraw((double) e.Data);

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
          

                //Ticker Events
                case "simulate":
                    return Simulate((string)e.Data);                   
              */
        #region AccountMethods
        [EventListenerAttr("deposit")]
        private Error Deposit(DepositWithdrawEvent e)
        {
            _mainModel.Account.Deposit(e.Amount);
            Broadcast(new DisplayEvent("account"));
            return Error.None;
        }

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

        /*[EventListenerAttr("deleteAllPortfolios")]
        private Error DeleteAllPortfolios(IEvent e)
        {
            return Error.None;
        }*/

        [EventListenerAttr("simulate")]
        private Error Simulate(IEvent e)
        {
            Broadcast(new DisplayEvent("account"));
            return Error.None;
        }
        #endregion AccountMethods

        #region PortfolioMethods
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


        [EventListenerAttr("newPortfolio")]
        private Error NewPortfolio(PortfolioEvent e)
        {
            _mainModel.Account.CreatePortfolio(e.PortfolioName, _mainModel.VerifyStock);
            _currentPortfolio = _mainModel.Account.GetPortfolioByName(e.PortfolioName);
            Broadcast(new PortfolioEvent("portfolio", _currentPortfolio.Name));
            return Error.None;
        }


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
