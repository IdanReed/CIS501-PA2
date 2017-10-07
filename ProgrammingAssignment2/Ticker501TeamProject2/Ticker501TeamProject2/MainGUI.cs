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
        #region OutputForm
        public void Update(Event e)
        {
            switch (e.Type)
            {
                case "accountStats":
                    uxTBFundsAmount.Text = "$" + _acct.Funds.ToString("N2");
                    UpdateAccPercentInfo();
                    break;
                case "portBuy": case "portSell":
                    UpdateAccStocksHeld();
                    break;
            }

        }
        private void UpdateAccPercentInfo()
        {
            double accCash = _acct.Funds;
            double accPosValue = 0;
            
            foreach (Portfolio port in _acct.Portfolios)
            {
                accPosValue += port.CashValue;
            }

            double accTotalValue = accPosValue + accCash;

            uxTBCashAmount.Text = "$" + String.Format("{0:0.00}", accCash.ToString());
            uxTBCashPercent.Text = String.Format("{0:0.00}", (accCash / accTotalValue)*100)+"%";

            uxTBPositionsAmount.Text = "$" + String.Format("{0:0.00}", accPosValue);
            uxTBPositonsPercent.Text = String.Format("{0:0.00}", (accPosValue / accTotalValue) * 100) + "%";
        }
        private void UpdateAccStocksHeld()
        {
            List<StockPurchase> allPurchases = new List<StockPurchase>();
            List<StockPurchase> combinedPurchases = new List<StockPurchase>();

            foreach (Portfolio port in _acct.Portfolios)
            {
                foreach(StockPurchase portStock in port.Stocks)
                {
                    allPurchases.Add(portStock);
                }
            }

            foreach (StockPurchase spOne in allPurchases)
            {
                StockPurchase selectedSP = spOne;
                allPurchases.Remove(spOne);
                foreach (StockPurchase spTwo in allPurchases)
                {
                    if (selectedSP.HasSameTicker(spTwo))
                    {
                        selectedSP = selectedSP.Add(spTwo);
                    }
                }
                combinedPurchases.Add(selectedSP);
            }
        }
        #endregion OutputForm

        #region InputForm
        private void DepositFunds(object sender, EventArgs e)
        {
            //Method that runs when the Deposit button is pressed
            double val = (double) uxNBFundsInput.Value;
            _inputHandle(new Event(val, "deposit"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("accountStats"));
        }

        private void WithdrawFunds(object sender, EventArgs e)
        {
            //Method that runs when the Withdraw button is pressed
            double val = (double)uxNBFundsInput.Value;
            _inputHandle(new Event(val, "withdraw"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("accountStats"));
        }

        private void NewPortfolio(object sender, EventArgs e)
        {
            //Method that runs when the New Portfolio button is pressed
        }

        private void DeletePortfolio(object sender, EventArgs e)
        {
            //Method that runs when the Delete Portfolio button is pressed
        }
        #endregion InputForm

    }
}
