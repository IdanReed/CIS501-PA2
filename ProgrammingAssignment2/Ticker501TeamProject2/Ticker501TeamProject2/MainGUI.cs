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
 
            foreach(Ticker t in _tickers)
            {
                uxDUDSelecStock.Items.Add(t.Tag + " - " + t.Price);
            }
            uxDUDSelecStock.SelectedIndex = 0;
            UpdateTickerList();

            uxDUDSelecVolatilty.Items.Add("HIGH");
            uxDUDSelecVolatilty.Items.Add("MED");
            uxDUDSelecVolatilty.Items.Add("LOW");
            uxDUDSelecVolatilty.SelectedIndex = 0;

            UpdateBuySellState();
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
                case "newPort": case "deletePort":
                    UpdatePortLBs();
                    UpdateBuySellState();
                    break;
                case "portStats":
                    UpdatePortStats(e.Data as Portfolio);
                    UpdateBuySellState();
                    break;
                case "showStocks":
                    UpdateTickerList();
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
            //Might not work right yet, untested
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

        private void UpdatePortLBs()
        {
            List<ListBox> boxes = new List<ListBox>();
            boxes.Add(uxLBPortfolios);
            boxes.Add(uxLBSelecPort);

            List<Portfolio> portfolios = _acct.Portfolios;

            foreach(ListBox lb in boxes)
            {
                lb.DataSource = portfolios.Select(p => p.Name).ToList();
            }
        }

        private void UpdatePortStats(Portfolio p)
        {
            List<string> portStats = new List<string>();
            
            if (p != null && p.Stocks.Count > 0)
            {
                foreach (StockPurchase s in p.Stocks)
                {
                    double percent = s.TotalPrice / p.CashValue;
                    double numPercent = s.Amount / (double)p.AmountStocks;

                   portStats.Add(s.TotalPrice.ToString("C") + "- Cash Value %: (" + String.Format("{0:P2}", percent) + ") - # Stocks (" + s.Amount + "): [" + String.Format("{0:P2}", numPercent) + "] - " + s.Ticker.Tag + " " + s.Ticker.Name);
                }
            }
            uxLBPortStocks.DataSource = portStats;
        }

        private void UpdateTickerList()
        {
            List<string> tickerStrings = new List<string>();
            foreach(Ticker ticker in _tickers)
            {
                tickerStrings.Add(ticker.Tag + " - " + ticker.Price);
            }
            uxLBAllStock.DataSource = tickerStrings;
        }
        private void UpdateBuySellState()
        {
            if(uxLBSelecPort.SelectedItem == null)
            {
                uxBBuyStock.Enabled = false;
                uxBSellStock.Enabled = false;
            }else
            {
                uxBBuyStock.Enabled = true;
                uxBSellStock.Enabled = true;
            }
        }
        #endregion OutputForm
        




        #region InputForm
        #region uxPanDepositWithdrawl
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
        #endregion uxPanDepositWithdrawl

        #region uxPanPortfoliosCreateDelete
        private void uxBNewPort_Click(object sender, EventArgs e)
        {
            _inputHandle(new Event(uxTBNewPortName.Text, "newPort"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            //_inputHandle(new Event("newPort"));
            
        }

        private void uxNewDelete_Click(object sender, EventArgs e)
        {

        }
        #endregion uxPanPortfoliosCreateDelete

        private void uxLBSelecPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _inputHandle(new Event(uxLBSelecPort.SelectedItem.ToString(), "portView"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("portStats"));
        }

        #region uxPanBuySellStock
        private void uxBuyStock_Click(object sender, EventArgs e)
        {
            string selectedStock = uxDUDSelecStock.SelectedItem.ToString().Split(new[] { ' ', '-' })[0];
            _inputHandle(new Event(new Tuple<string, int>( selectedStock, (int)uxNUDStockQuanity.Value), "portBuyShares"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("portStats"));
            _inputHandle(new Event("accountStats"));
        }
        private void uxBSellStock_Click(object sender, EventArgs e)
        {
            string selectedStock = uxDUDSelecStock.SelectedItem.ToString().Split(new[] { ' ', '-' })[0];
            _inputHandle(new Event(new Tuple<string, int>(selectedStock, (int)uxNUDStockQuanity.Value), "portSell"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("portStats"));
            _inputHandle(new Event("accountStats"));
        }
        #endregion uxPanBuySellStock



        private void uxBSimulatePrice_Click(object sender, EventArgs e)
        {
            _inputHandle(new Event(uxDUDSelecVolatilty.SelectedItem.ToString(), "simulate"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("portStats"));
            _inputHandle(new Event("accountStats"));
        }
        #endregion InputForm


    }
}
