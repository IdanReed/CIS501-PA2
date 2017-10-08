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

            UpdateSelectStock();
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
                    UpdateAccStocksHeld();
                    break;
                case "portBuy": case "portSell":
                    UpdateAccStocksHeld();
                    break;
                case "newPort": case "deletePort":
                    UpdatePortLBs();
                    UpdateBuySellState();
                    UpdateAccStocksHeld();
                    
                    break;
                case "portStats":
                    UpdatePortStats(e.Data as Portfolio);
                    UpdateBuySellState();
                    UpdateAccStocksHeld();
                    UpdatePortInfo(e.Data as Portfolio);
                    break;
                case "showStocks":
                    UpdateTickerList();
                    UpdateSelectStock();
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

            uxTBCashAmount.Text = "$" + String.Format("{0:0.0}", accCash.ToString());
            uxTBCashPercent.Text = String.Format("{0:0.0}", (accCash / accTotalValue)*100)+"%";

            uxTBPositionsAmount.Text = "$" + String.Format("{0:0.0}", accPosValue);
            uxTBPositonsPercent.Text = String.Format("{0:0.0}", (accPosValue / accTotalValue) * 100) + "%";
        }

        private void UpdateAccStocksHeld()
        {
            //Might not work right yet, untested
            List<StockPurchase> allPurchases = new List<StockPurchase>();
            List<StockPurchase> combinedPurchases = new List<StockPurchase>();
            
            foreach (Portfolio port in _acct.Portfolios)
            {
                foreach(StockPurchase sp in port.Stocks)
                {
                    allPurchases.Add(sp);
                }
            }

            if(allPurchases.Count > 0)
            {
                foreach (StockPurchase sp in allPurchases)
                {
                    StockPurchase selectedSP = sp;
                    //allPurchases.Remove(sp);
                    foreach (StockPurchase spTwo in allPurchases)
                    {
                        if (selectedSP.HasSameTicker(spTwo))
                        {
                            selectedSP = selectedSP.Add(spTwo);
                        }
                    }
                    combinedPurchases.Add(selectedSP);
                    if(allPurchases.Count == 0) { break; }
                }
            }
            uxLBStocksHeld.DataSource = combinedPurchases;
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
                   portStats.Add(s.Ticker.Tag + " - $" + String.Format("{0:0.00}", s.Ticker.Price*s.Amount) + " - %" + String.Format("{0:0.00}", (s.Ticker.Price * s.Amount / p.CashValue)*100) + " - " + s.Amount);
                }
            }
            uxLBPortStocks.DataSource = portStats;
        }
        /*
        private void UpdateAccStockHeld()
        {
            foreach(Portfolio p in _acct.Portfolios)
            {
                List<string> portStats = new List<string>();

                if (p != null && p.Stocks.Count > 0)
                {
                    foreach (StockPurchase s in p.Stocks)
                    {
                        portStats.Add(s.Ticker.Tag + " - $" + String.Format("{0:0.00}", s.Ticker.Price * s.Amount) + " - %" + String.Format("{0:0.00}", (s.Ticker.Price * s.Amount / p.CashValue) * 100) + " - " + s.Amount);
                    }
                }
                uxLBStocksHeld.DataSource = portStats;
            }
        }
        */
        private void UpdateTickerList()
        {
            List<string> tickerStrings = new List<string>();
            foreach(Ticker ticker in _tickers)
            {
                tickerStrings.Add(ticker.Tag + " - " + String.Format("{0:0.00}",ticker.Price));
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

        private void UpdateSelectStock()
        {
            uxDUDSelecStock.Items.Clear();

            foreach (Ticker t in _tickers)
            {
                uxDUDSelecStock.Items.Add(t.Tag + " - " + String.Format("{0:0.00}", t.Price));
            }
            //This dumb, but it won't update
            uxDUDSelecStock.SelectedIndex = 0;
            uxDUDSelecStock.SelectedIndex = 1;
            uxDUDSelecStock.SelectedIndex = 0;
        }

        private void UpdatePortInfo(Portfolio p)
        {
            
            double accountValue = 0;
            foreach(Portfolio port in _acct.Portfolios)
            {
                accountValue += port.CashValue;
            }
            uxTBAmountInvested.Text = String.Format("{0:0.00}",p.CashValue);
            uxTBPortPercentOfAcc.Text = String.Format("{0:0.00}", (p.CashValue/accountValue * 100)) + "%";
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
            
        }

        private void uxNewDelete_Click(object sender, EventArgs e)
        {
            _inputHandle(new Event(uxLBPortfolios.SelectedItem.ToString(), "deletePort"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
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
