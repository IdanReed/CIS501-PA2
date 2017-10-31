﻿//using Class_Library;
using ModelRebuild;
using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticker501TeamProject3
{
    public partial class MainGUI : Form
    {
        /*private InputHandler _inputHandle;
        private Account _acct;
        private List<Ticker> _tickers;*/

        private EventListener<Error> _eventListener;
        private MVCEventSystem.EventHandler<Error> _handler;
        public MVCEventSystem.EventHandler<Error> EventHandler
        {
            get { return _handler; }
        }
        private MainModel _mainModel;
        /// <summary>
        /// Sets up the class varibles also fills volitiliy select and makes sure that the correct buttons are enabled.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="tickers"></param>
        /// <param name="inputHander"></param>
        public MainGUI(EventListener<Error> listener, MainModel mm)
        {

            _mainModel = mm;
            _eventListener = listener;

            _handler = new MVCEventSystem.EventHandler<Error>();
            _handler.AddEventListener<DisplayEvent>(typeof(DisplayEvent), Update);
            _handler.AddEventListener<PortfolioEvent>(typeof(PortfolioEvent), UpdatePortfolio);
           /* _inputHandle = inputHander;
            _acct = a;
            _tickers = tickers;
*/
            InitializeComponent();
            /*
            UpdateSelectStock();
            UpdateTickerList();
            */
            uxDUDSelecVolatilty.Items.Add("HIGH");
            uxDUDSelecVolatilty.Items.Add("MED");
            uxDUDSelecVolatilty.Items.Add("LOW");
            uxDUDSelecVolatilty.SelectedIndex = 0;

            //UpdateBuySellState();
        }
        //#region OutputForm
        /// <summary>
        /// Where all the output triggering events from the controller end up.
        /// </summary>
        /// <param name="e"></param>
        /// 
        public Error Update(DisplayEvent e)
        {
            #region DepositWithdrawPanel
            uxTBFundsAmount.Text = _mainModel.Account.Funds.ToString("N2");

            double accTotalValue = _mainModel.Account.CalculateValue(_mainModel.Stocks);
            double accPosValue = accTotalValue - _mainModel.Account.Funds;
            if (!(accTotalValue == 0))
            {
                uxTBCashPercent.Text = String.Format("{0:0.0}", (_mainModel.Account.Funds / accTotalValue) * 100) + "%";
                uxTBPositonsPercent.Text = String.Format("{0:0.0}", (accPosValue / accTotalValue) * 100) + "%";
            }

            uxTBPositionsAmount.Text = "$" + String.Format("{0:0.0}", accPosValue);

            #endregion DepositWithdrawPanel


            return Error.None;
        }
        public Error UpdatePortfolio(PortfolioEvent e)
        {

            #region PortfoliosPanel

            List<ListBox> boxes = new List<ListBox>();
            boxes.Add(uxLBPortfolios);
            boxes.Add(uxLBSelecPort);

            List<Portfolio> portfolios = _mainModel.Account.Portfolios;

            foreach(ListBox lb in boxes)
            {
                lb.DataSource = portfolios.Select(p => p.Name).ToList();
            }

            #endregion PortfoliosPanel

            return Error.None;
        }
        /*
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
                    UpdatePortStocks(e.Data as Portfolio);
                    UpdateAccGainsLosses();
                    UpdatePortGainsLosses(e.Data as Portfolio);
                    UpdatePortInfo(e.Data as Portfolio);
                    uxTBFundsAmount.Text = "$" + _acct.Funds.ToString("N2");
                    break;
                case "portStats":
                    UpdatePortStocks(e.Data as Portfolio);
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
        /// <summary>
        /// Fills in the cash and positions amount and the percents each out of total value.
        /// </summary>
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

            if (!(accTotalValue == 0))
            {
                uxTBCashPercent.Text = String.Format("{0:0.0}", (accCash / accTotalValue) * 100) + "%";
            }

            uxTBPositionsAmount.Text = "$" + String.Format("{0:0.0}", accPosValue);

            if(!(accTotalValue == 0))
            {
                uxTBPositonsPercent.Text = String.Format("{0:0.0}", (accPosValue / accTotalValue) * 100) + "%";
            }
            


            UpdateAccGainsLosses();
        }
        /// <summary>
        /// This goes through all the portfolios that have been created and gets all the stock purchases. I then combines those with the same ticker.
        /// </summary>
        private void UpdateAccStocksHeld()
        {
            List<string> allPurchases = new List<string>();
            
            foreach (Portfolio port in _acct.Portfolios)
            {
                foreach(StockPurchase s in port.Stocks)
                {
                    allPurchases.Add(s.Ticker.Tag + " - $" + String.Format("{0:0.00}", s.Ticker.Price * s.Amount) + " - " + String.Format("{0:0.00}", (s.Ticker.Price * s.Amount / _acct.CurValue) * 100) + "% - " + s.Amount);
                }
            }

           
            uxLBStocksHeld.DataSource = allPurchases;
        }

        /// <summary>
        /// Goes through and adds the created portfolios to the create delete and to select portfolios
        /// </summary>
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

        /// <summary>
        /// Adds the selected portfolios in purchased stocks to the portfolio list box 
        /// </summary>
        /// <param name="p"></param>
        private void UpdatePortStocks(Portfolio p)
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
       /// <summary>
       /// Only runs at start and with simulate. Updates the listbox with new tickers 
       /// </summary>
        private void UpdateTickerList()
        {
            List<string> tickerStrings = new List<string>();
            foreach(Ticker ticker in _tickers)
            {
                tickerStrings.Add(ticker.Tag + " - " + String.Format("{0:0.00}",ticker.Price));
            }
            uxLBAllStock.DataSource = tickerStrings;
        }
        /// <summary>
        /// Keeps the buy sell buttons disabled when there is no portfolio selected
        /// </summary>
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
        /// <summary>
        /// Adds the tickers to be bought or sold in the DUD. Being a pain to update.
        /// </summary>
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
        /// <summary>
        /// Sets the percents for the portfolio
        /// </summary>
        /// <param name="p"></param>
        private void UpdatePortInfo(Portfolio p)
        {
            if(p == null)
            {
                uxTBPortPercentOfAcc.Text = "";
                uxTBAmountInvested.Text = "";
            }
            else
            {
                double accountValue = 0;
                foreach (Portfolio port in _acct.Portfolios)
                {
                    accountValue += port.CashValue;
                }
                uxTBAmountInvested.Text = String.Format("{0:0.00}", p.CashValue);

                if (!(accountValue == 0)) { uxTBPortPercentOfAcc.Text = String.Format("{0:0.00}", (p.CashValue / accountValue * 100)) + "%"; }

                UpdatePortGainsLosses(p);
            }
            
        }

        /// <summary>
        /// Sets the TB with the gain loss for the account
        /// </summary>
        private void UpdateAccGainsLosses()
        {
            if (_acct.ChangeInFunds < 0)
            {
                uxTBGainLoss.Text = "-" + _acct.ChangeInFunds.ToString("C");
            }
            else
                uxTBGainLoss.Text = _acct.ChangeInFunds.ToString("C");
        }
        /// <summary>
        /// Sets the TB with the gain loss for the portfolio
        /// </summary>
        /// <param name="selectedPortfolio"></param>
        private void UpdatePortGainsLosses(Portfolio selectedPortfolio)
        {
            //Portfolio selectedPortfolio = uxLBSelecPort.SelectedItem as Portfolio;
            if (selectedPortfolio?.ChangeInValue < 0)
            {
                uxTBPortGainLoss.Text = "-" + selectedPortfolio.ChangeInValue.ToString("C");
            }
            else
                uxTBPortGainLoss.Text = selectedPortfolio?.ChangeInValue.ToString("C");
        }
        #endregion OutputForm 
        */

        #region InputForm
        #region uxPanDepositWithdrawl
        private void DepositFunds(object sender, EventArgs e)
        {
            _eventListener(new DepositWithdrawEvent("deposit", (double)uxNBFundsInput.Value))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }
        private void WithdrawFunds(object sender, EventArgs e)
        {

            _eventListener(new DepositWithdrawEvent("withdraw", (double)uxNBFundsInput.Value))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }

        #endregion uxPanDepositWithdrawl

        #region uxPanPortfoliosCreateDelete
        private void uxBNewPort_Click(object sender, EventArgs e)
        {
            _eventListener(new PortfolioEvent("newPortfolio", uxTBNewPortName.Text))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });            
        }

        private void uxNewDelete_Click(object sender, EventArgs e)
        {
            _eventListener(new PortfolioEvent("deletePortfolio", uxLBPortfolios.SelectedItem.ToString()))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }
        #endregion uxPanPortfoliosCreateDelete
        /// <summary>
        /// Triggers when the selected port is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxLBSelecPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _eventListener(new PortfolioEvent("viewPortfolio", uxLBSelecPort.SelectedItem.ToString()))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }

        #region uxPanBuySellStock
        /// <summary>
        /// Triggers the buy event and causes updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxBuyStock_Click(object sender, EventArgs e)
        {
            string selectedStock = uxDUDSelecStock.SelectedItem.ToString().Split(new[] { ' ', '-' })[0];
            _eventListener(new StockEvent("buyStocks", selectedStock, (int)uxNUDStockQuanity.Value))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }
        /// <summary>
        /// Triggers the sell event and causes updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxBSellStock_Click(object sender, EventArgs e)
        {
            string selectedStock = uxDUDSelecStock.SelectedItem.ToString().Split(new[] { ' ', '-' })[0];
            _eventListener(new StockEvent("sellStocks", selectedStock, (int)uxNUDStockQuanity.Value))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
        }
        #endregion uxPanBuySellStock

        /// <summary>
        /// Triggers the simulate event and causes updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxBSimulatePrice_Click(object sender, EventArgs e)
        {
            _eventListener(new SimulateEvent("simulate", uxDUDSelecVolatilty.SelectedItem.ToString()))
                .Catch(error =>
                {
                    MessageBox.Show(error.Message);
                });
            /*_inputHandle(new Event(uxDUDSelecVolatilty.SelectedItem.ToString(), "simulate"))
                .Catch(message =>
                {
                    MessageBox.Show(message);
                }
            );
            _inputHandle(new Event("portStats"));
            _inputHandle(new Event("accountStats"));*/
        }

        #endregion InputForm

        private void MainGUI_Load(object sender, EventArgs e)
        {

        }
    }
}
