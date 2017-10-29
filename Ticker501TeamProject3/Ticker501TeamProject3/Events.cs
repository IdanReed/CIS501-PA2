using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501TeamProject3
{
    class DisplayEvent : IEvent
    {
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
        }
        public DisplayEvent(string type)
        {
            _type = type;
        }
    }

    class DepositWithdrawEvent : IEvent
    {
        private string _type;
        private double _amount;
        public string Type
        {
            get
            {
                return _type;
            }
        }
        public double Amount
        {
            get { return _amount; }
        }

        public DepositWithdrawEvent(string type, double amount)
        {
            _type = type;
            _amount = amount;
        }
    }

    class PortfolioEvent: IEvent
    {
        private string _type;
        private string _portfolioName;

        public string Type
        {
            get { return _type; }
        }
        public string PortfolioName
        {
            get { return _portfolioName; }
        }
        public PortfolioEvent(string type, string name)
        {
            _type = type;
            _portfolioName = name;
        }
    }

    class StockEvent: IEvent
    {
        private string _type;
        private int _amount;
        private string _name;
        public string Type
        {
            get { return _type; }
        }
        public int Amount
        {
            get { return _amount; }
        }
        public string Name
        {
            get { return _name; }
        }
        public StockEvent(string type, string name, int amount)
        {
            _type = type;
            _name = name;
            _amount = amount;
        }
    }
}
