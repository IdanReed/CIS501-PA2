using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501TeamProject3
{
    class Controller: MVCEventSystem.Broadcaster<Error>
    {
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
            
            return Error.None;
        }

        [EventListenerAttr("withdraw")]
        private Error Withdraw(DepositWithdrawEvent e)
        {
            return Error.None;
        }

        [EventListenerAttr("deleteAllPortfolios")]
        private Error DeleteAllPortfolios(IEvent e)
        {
            return Error.None;
        }

        [EventListenerAttr("simulate")]
        private Error Simulate(IEvent e)
        {
            return Error.None;
        }
        #endregion AccountMethods

        #region PortfolioMethods
        [EventListenerAttr("deletePortfolio")]
        private Error DeletePortfolio(PortfolioEvent e)
        {
            return Error.None;
        }


        [EventListenerAttr("newPortfolio")]
        private Error NewPortfolio(PortfolioEvent e)
        {
            return Error.None;
        }


        [EventListenerAttr("viewPortfolio")]
        private Error ViewPortfolio(PortfolioEvent e)
        {
            return Error.None;
        }


        [EventListenerAttr("sellStocks")]
        private Error SellStocks(StockEvent e)
        {
            return Error.None;
        }
        [EventListenerAttr("buyStocks")]
        private Error BuyStocks(StockEvent e)
        {
            return Error.None;
        }

        #endregion PortfolioMethods
    }
}
