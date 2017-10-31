using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    /// <summary>
    /// The event sent when the display needs updating
    /// </summary>
    public class DisplayEvent : IEvent
    {
        private string _type;
        /// <summary>
        /// Gets the type of the event. Implments IEvent
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
        }
        /// <summary>
        /// Creates a DisplayEvent with a type
        /// </summary>
        /// <param name="type">The type of the event</param>
        public DisplayEvent(string type)
        {
            _type = type;
        }
    }

    /// <summary>
    /// The event sent when the user wants to deposit of withdraw money
    /// </summary>
    public class DepositWithdrawEvent : IEvent
    {
        private string _type;
        private double _amount;
        /// <summary>
        /// They type of the event. Implements IEvent
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
        }
        /// <summary>
        /// Gets the amount of money to deposit or withdraw
        /// </summary>
        public double Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// Creates a DepositWithdrawEvent with a type and an amount to deposit/withdraw
        /// </summary>
        /// <param name="type">The type of event</param>
        /// <param name="amount">The amount to deposit/withdraw</param>
        public DepositWithdrawEvent(string type, double amount)
        {
            _type = type;
            _amount = amount;
        }
    }

    /// <summary>
    /// The event sent when editing a portfolio
    /// </summary>
    public class PortfolioEvent: IEvent
    {
        private string _type;
        private string _portfolioName;

        /// <summary>
        /// Gets the type of the event. Implements IEvent
        /// </summary>
        public string Type
        {
            get { return _type; }
        }
        /// <summary>
        /// Gets the name of the portfolio to edit
        /// </summary>
        public string PortfolioName
        {
            get { return _portfolioName; }
        }
        /// <summary>
        /// Creates a PortfolioEvent with a type and the name of the portfolio
        /// </summary>
        /// <param name="type">The type of the event</param>
        /// <param name="name">The name of the portfolio</param>
        public PortfolioEvent(string type, string name)
        {
            _type = type;
            _portfolioName = name;
        }
    }

    /// <summary>
    /// The event sent when the user wants to buy or sell stock
    /// </summary>
    public class StockEvent: IEvent
    {
        private string _type;
        private int _amount;
        private string _name;

        /// <summary>
        /// Gets the type of the event. Implements IEvent
        /// </summary>
        public string Type
        {
            get { return _type; }
        }
        /// <summary>
        /// Gets the amount of stock to buy
        /// </summary>
        public int Amount
        {
            get { return _amount; }
        }
        /// <summary>
        /// Gets the name of the stock to buy
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// Creates a stock event with a type, name, and amount
        /// </summary>
        /// <param name="type">The type of event</param>
        /// <param name="name">The name of the stock to buy</param>
        /// <param name="amount">The amount of shares to buy</param>
        public StockEvent(string type, string name, int amount)
        {
            _type = type;
            _name = name;
            _amount = amount;
        }
    }

    /// <summary>
    /// The event sent when the user wants to simulate prices
    /// </summary>
    public class SimulateEvent: IEvent
    {
        private string _type;
        private string _vol;

        /// <summary>
        /// Gets the type of the event. Implements IEvent
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the volatility of the simulation
        /// </summary>
        public string Volatility
        {
            get { return _vol; }
        }
        /// <summary>
        /// Creates a simulate event with a type and volatility
        /// </summary>
        /// <param name="type">The type of the event</param>
        /// <param name="vol">The volatilty</param>
        public SimulateEvent(string type, string vol)
        {
            _vol = vol;
            _type = type;
        }
    }
}
