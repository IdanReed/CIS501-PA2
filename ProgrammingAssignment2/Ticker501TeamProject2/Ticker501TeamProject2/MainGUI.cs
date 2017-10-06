using Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticker501TeamProject2
{
    public partial class MainGUI : Form
    {
        private InputHandler _inputHandle;
        private Account _acct;
        private List<Ticker> _tickers;

        public MainGUI(Account a, List<Ticker> tickers, InputHandler inputHander)
        {
            _inputHandle = inputHander;
            _acct = a;
            _tickers = tickers;

            InitializeComponent();
        }

        public void Update(Event e)
        {

        }

        private void DepositFunds(object sender, EventArgs e)
        {
            //Method that runs when the Deposit button is pressed
        }

        private void WithdrawFunds(object sender, EventArgs e)
        {
            //Method that runs when the Withdraw button is pressed
        }

        private void NewPortfolio(object sender, EventArgs e)
        {
            //Method that runs when the New Portfolio button is pressed
        }

        private void DeletePortfolio(object sender, EventArgs e)
        {
            //Method that runs when the Delete Portfolio button is pressed
        }
    }
}
