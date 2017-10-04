using Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    class OutputView
    {

        private Account _acct;
        private List<Ticker> _tickers;
        public OutputView(Account a, List<Ticker> tickers)
        {
            _acct = a;
            _tickers = tickers;
        }
        public void Update(Event e)
        {
            switch (e.Type)
            {
                //Respond to an event sent from Controller
                case "accountStats":
                    DisplayAccount();
                    break;
            }            
        }

        private void DisplayAccount()
        {
            //Do display stuff
            Console.WriteLine("Funds: ${0}",_acct.Funds.ToString("N2"));
            MenuHelper.PressEnter();
        }
    }
}
