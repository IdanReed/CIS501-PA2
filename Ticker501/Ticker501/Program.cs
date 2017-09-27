///Jarod Honas
///501SF17
///A1
///Program
///Controls the UI and creates instances of the helper classes


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Program
    {
        static List<Ticker> tickers = new List<Ticker>();
        static double funds = 0;

        /// <summary>
        /// Main
        /// Controls the UI menu
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            tickers = getTickers();
            Simulation sim = new Simulation("low");
            Portfolio[] portfolios = new Portfolio[3];
            

            Console.WriteLine("Welcome to Ticker501. Please add funds to start your portfolios or type \"help\" for a list of commands");



            string input = "";
            while(input != "exit")
            {
                Console.Write("Command: ");
                input = Console.ReadLine();
                if (input == "help")
                {
                    Console.WriteLine("\nexit: exits the program\\the menu you are in\ndeposit: add funds to your account\nwithdraw: withdraw funds from your account\nupdate: updates prices for stocks\ndelete: deletes a portfolio\nnew: creates a portfolio");
                    Console.WriteLine("edit: choose a portfolio to edit\nview: view all of your portfolios\n");

                }
                else if (input == "deposit")
                {
                    Deposit();
                }
                else if (input == "withdraw")
                {
                    Withdraw();
                }
                else if (input == "update")
                {
                    Update(sim);
                }
                else if (input == "delete")
                {
                    Delete(portfolios);
                }
                else if (input == "new")
                {
                    New(portfolios);
                }
                else if (input == "edit")
                {
                    Edit(portfolios);
                }
                else if(input == "view")
                {
                    View(portfolios);
                }
                else
                {
                    Console.WriteLine("Command not recognized. Type \"help\" for a list of commands");
                }
            }//end input loop
        }//end main



        #region MainMethods
        /// <summary>
        /// Deposit
        /// Deposits the user given amount of cash into their funds
        /// </summary>
        static void Deposit()
        {
            bool valid = false;
            while (valid == false)
            {
                Console.Write("Funds to add: ");
                try
                {
                    double tempFunds = -4.99;
                    string input = Console.ReadLine();
                    if (input == "exit") return;
                    tempFunds += Convert.ToDouble(input);
                    if (tempFunds > 0)
                    {
                        valid = true;
                        funds += tempFunds;
                    }
                    else
                    {
                        Console.WriteLine("The entry was invalid! Please only enter positive numbers.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The entry was invalid! Please only enter positive numbers.");
                }
            }
            Console.WriteLine("A fee of $4.99 has been deducted");
            Console.WriteLine("Your current balance: " + funds.ToString("C"));
        }

        /// <summary>
        /// Withdraw
        /// Withdraws the user given amount of cash from their funds
        /// </summary>
        static void Withdraw()
        {
            bool valid = false;
            while (valid == false)
            {
                Console.Write("Funds to withdraw: ");
                try
                {
                    double tempFunds = 4.99;
                    string input = Console.ReadLine();
                    if (input == "exit") return;
                    tempFunds += Convert.ToDouble(input);
                    if (tempFunds > 0 && (funds - tempFunds) > 0)
                    {
                        valid = true;
                        funds -= tempFunds;
                    }
                    else
                    {
                        Console.WriteLine("The entry was invalid! Please only enter positive numbers and you cannot withdraw more funds than you have in your \nbalance.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The entry was invalid! Please only enter positive numbers.");
                }
            }
            Console.WriteLine("A fee of $4.99 has been deducted");
            Console.WriteLine("Your current balance: " + funds.ToString("C"));
        }

        /// <summary>
        /// Update
        /// Updates the prices of the stocks based on the volatility of the market
        /// </summary>
        /// <param name="sim"></param>
        static void Update(Simulation sim)
        {
            bool valid = false;
            while (valid == false)
            {
                Console.Write("Enter the volatility for the market (high, med, low): ");
                string input = Console.ReadLine();
                if (input == "high" || input == "med" || input == "low")
                {
                    valid = true;
                    sim.Volatility = input;
                    foreach (Ticker t in tickers)
                    {
                        t.UpdatePrice(sim);
                    }
                    Console.WriteLine("Prices Updated!");
                }
                else if (input == "exit") return;
                else
                {
                    Console.WriteLine("Entry invalid!");
                }
            }
        }

        /// <summary>
        /// Deletes a portfolio and sells all of its stocks which go back into the funds
        /// </summary>
        /// <param name="portfolios"></param>
        static void Delete(Portfolio[] portfolios)
        {
            if (portfolios[0] == null && portfolios[1] == null && portfolios[2] == null)
            {
                Console.WriteLine("You do not have any portfolios! Use command \"new\" to create a portfolio.");
            }
            else
            {
                bool found = false;
                while (found == false)
                {
                    Console.WriteLine("Enter the name of the portfolio you want to delete.");
                    for (int i = 0; i < 3; i++)
                    {
                        if (portfolios[i] != null)
                        {
                            Console.WriteLine("Portfolio " + (i + 1) + ": " + portfolios[i].Name);
                        }
                    }
                    Console.Write("Name: ");
                    string input = Console.ReadLine();
                    if (input == "exit") return;
                    for (int i = 0; i < 3; i++)
                    {
                        if (portfolios[i] != null)
                        {
                            if (portfolios[i].Name == input)
                            {
                                Console.WriteLine("Portfolio " + input + " had been deleted.");
                                double val = 0;
                                foreach (Stock s in portfolios[i].stocks)
                                {
                                    val += s.TotalPrice;
                                }
                                funds += val;
                                Console.WriteLine("Your current balance: " + funds.ToString("C"));
                                portfolios[i] = null;
                                found = true;
                            }
                            else
                            {
                                Console.WriteLine("Name not found!");
                            }
                            if (found) break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// creates a new portfolio
        /// </summary>
        /// <param name="portfolios"></param>
        static void New(Portfolio[] portfolios)
        {
            if (portfolios[0] == null || portfolios[1] == null || portfolios[2] == null)
            {
                bool found = false;
                for (int i = 0; i < 3; i++)
                {

                    if (portfolios[i] == null)
                    {
                        found = true;
                        Console.Write("The next open portfolio is #" + (i + 1) + ". Enter the name for the portfolio: ");
                        portfolios[i] = new Portfolio();
                        string input = Console.ReadLine();
                        if (input == "exit") return;
                        portfolios[i].Name = input;
                        Console.WriteLine("You have created portfolio \"" + portfolios[i].Name + "\".");
                    }
                    if (found) break;
                }
            }
            else
            {
                Console.WriteLine("You already have created your maximum amount of portfolios(3)!");
            }
        }

        /// <summary>
        /// goes into the editing mode of the selected portfolio
        /// </summary>
        /// <param name="portfolios"></param>
        static void Edit(Portfolio[] portfolios)
        {
            if (portfolios[0] == null && portfolios[1] == null && portfolios[2] == null)
            {
                Console.WriteLine("You do not have any portfolios! Use command \"new\" to create a portfolio.");
            }
            else
            {
                bool found = false;
                while (found == false)
                {
                    Console.WriteLine("Enter the name of the portfolio you want to edit.");
                    for (int i = 0; i < 3; i++)
                    {
                        if (portfolios[i] != null)
                        {
                            Console.WriteLine("\tPortfolio " + (i + 1) + ": " + portfolios[i].Name);
                        }
                    }
                    Console.Write("Name: ");
                    string input = Console.ReadLine();
                    if (input == "exit") return;
                    for (int i = 0; i < 3; i++)
                    {
                        if (portfolios[i] != null)
                        {
                            if (portfolios[i].Name == input)
                            {
                                Console.WriteLine("Now editing portfolio " + input);
                                found = true;
                                EditPortfolio(portfolios[i]);
                            }
                            else
                            {
                                Console.WriteLine("Name not found!");
                            }
                            if (found) break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// View
        /// views the content of all of your portfolios
        /// </summary>
        /// <param name="portfolios"></param>
        static void View(Portfolio[] portfolios)
        {
            if (portfolios[0] == null)
            {
                Console.WriteLine("You do not have any portfolios! Use command \"new\" to create a portfolio.");
            }
            else
            {
                Console.WriteLine("Your Portfolios: ");
                for (int i = 0; i < 3; i++)
                {
                    if (portfolios[i] != null)
                    {
                        Console.WriteLine(portfolios[i].ToString());
                    }
                }
            }
        }
        #endregion

        #region PortfolioMethods

        /// <summary>
        /// PortView
        /// views the specific portfolio
        /// </summary>
        /// <param name="port"></param>
        static void PortView(Portfolio port)
        {
            bool found = false;
            Console.WriteLine("Portfolio " + port.Name + ": ");
            foreach (Stock s in port.stocks)
            {
                found = true;
                double percent = Convert.ToDouble(s.TotalPrice) / Convert.ToDouble(port.CashValue);
                double numPercent = Convert.ToDouble(s.Amount) / Convert.ToDouble(port.AmountStocks);
                Console.WriteLine("\t"+s.TotalPrice.ToString("C") + "\t- Cash Value %: (" + String.Format("{0:P2}", percent) + ") - # Stocks ("+s.Amount+"): ["+ String.Format("{0:P2}", numPercent) + "] - "+ s.ticker.Tag + " " + s.ticker.Name);
            }
            if (!found) Console.WriteLine("You have no stocks to view. Use \"buy\" to start investing.");
        }

        /// <summary>
        /// PortBuy
        /// Buys stocks and stores them in the portfolio
        /// </summary>
        /// <param name="port"></param>
        static void PortBuy(Portfolio port)
        {
            bool temp = false;
            while (temp == false)
            {
                Console.WriteLine("Stocks Available: ");

                foreach (Ticker t in tickers)
                {
                    Console.WriteLine("\t" + t.ToString());
                }
                bool found = false;
                Ticker tempTick = new Ticker();

                while (found == false)
                {
                    Console.Write("Enter ticker of stock to buy: ");
                    string tick = Console.ReadLine();
                    if (tick == "exit") return;
                    foreach (Ticker t in tickers)
                    {
                        if (t.Tag == tick.ToUpper())
                        {
                            if (port.stocks.Count == 0)
                            {
                                found = true;
                                tempTick = t;
                            }
                            foreach (Stock s in port.stocks)
                            {
                                if (s.ticker.Tag == t.Tag)
                                {
                                    Console.WriteLine("You cannot buy stock from a company you already have stock in: (" + t.Tag + ")");
                                }
                                else
                                {
                                    tempTick = t;
                                    found = true;
                                }
                            }
                        }
                    }
                    if (found == false) Console.WriteLine("Ticker not found!");
                }
                int amount = 0;
                bool valid = false;
                while (!valid)
                {
                    Console.Write("Enter amount of stock to buy: ");
                    try
                    {
                        string input = Console.ReadLine();
                        if (input == "exit") return;
                        amount = Convert.ToInt32(input);
                        if (amount >= 1) valid = true;
                    }
                    catch (Exception)
                    {

                    }
                    if (!valid) Console.WriteLine("Entry Invalid! Only enter positive integers.");
                    else
                    {
                        double price = 9.99;
                        price += tempTick.Price * amount;
                        if (price > funds)
                        {
                            Console.WriteLine("You have insufficient funds for this transaction.");
                            temp = true;
                            valid = false;
                        }
                        else
                        {
                            port.AmountStocks += amount;
                            Stock stock = new Stock(tempTick, amount);
                            port.stocks.Add(stock);
                            funds -= price;
                            Console.WriteLine("A transfer fee of $9.99 has been added to your purchase.");
                            Console.WriteLine("You have bought " + amount + " stocks in " + tempTick.Name + " for a price of " + price.ToString("C"));
                            Console.WriteLine("Your current funds: " + funds.ToString("C"));
                            temp = true;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// PortSell
        /// Sells stocks and returns cash to funds
        /// </summary>
        /// <param name="port"></param>
        static void PortSell(Portfolio port)
        {
            if (port.stocks.Count == 0)
            {
                Console.WriteLine("You have no stocks to view. Use \"buy\" to start investing.");
            }
            else
            {
                Console.WriteLine("Stocks available to sell: ");
                foreach (Stock s in port.stocks)
                {
                    Console.WriteLine("\t" + s.ToString());
                }

                bool found = false;

                while (!found)
                {
                    Console.Write("Enter ticker for stock to sell: ");
                    string tick = Console.ReadLine();
                    if (tick == "exit") return;
                    Stock sellStock = new Stock();
                    foreach (Stock s in port.stocks)
                    {

                        if (s.ticker.Tag == tick)
                        {
                            funds += s.TotalPrice - 9.99;
                            Console.WriteLine("A fee of 9.99 has been deducted for the transfer.");
                            Console.WriteLine("Your current balance: " + funds.ToString("C"));
                            port.stocks.Remove(s);
                            found = true;
                        }
                        if (found) break;

                    }
                    if (!found)
                    {
                        Console.WriteLine("Ticker not found!");
                    }
                }
            }
        }

        #endregion



        /// <summary>
        /// Gives the options to view the specific portfolio, buy stocks, and sell stocks
        /// </summary>
        /// <param name="port"></param>
        /// <param name="funds"></param>
        /// <returns></returns>
        static void EditPortfolio(Portfolio port)
        {
            Console.WriteLine("Buy or sells stock for portfolio: " + port.Name);
            string input = "";
            while(input != "exit")
            {
                Console.Write("Portfolio Command: ");
                input = Console.ReadLine();
                if(input == "help")
                {
                    Console.WriteLine("view: views the stocks in the portfolio\nbuy: buys stocks for the portfolio\nsell: sells stocks in the portfolio");
                    Console.WriteLine("exit: exits the portfolio\\the menu you are in");
                }
                else if(input == "view")
                {
                    PortView(port);
                }//end view
                else if(input == "buy")
                {
                    PortBuy(port);
                }//end buy
                else if(input == "sell")
                {
                    PortSell(port);
                }//end sell
                else
                {
                    Console.WriteLine("Command not recognized. Type \"help\" for a list of commands");
                }
            }
            Console.WriteLine("Portfolio exited.");
            
        }//end edit portfolio


        /// <summary>
        /// reads the database file and loads the data into tickers
        /// returns a list of tickers
        /// </summary>
        /// <returns></returns>
        static List<Ticker> getTickers()
        {
            List<Ticker> tempList = new List<Ticker>();
            StreamReader sr = new StreamReader("Ticker.txt");
            string line = sr.ReadLine();
            while(line != "")
            {
                Ticker tempTicker = new Ticker501.Ticker();
                string[] pieces = line.Split('-');
                tempTicker.Tag = pieces[0];
                tempTicker.Name = pieces[1];
                tempTicker.Price = Convert.ToDouble(pieces[2].Substring(1));
                tempList.Add(tempTicker);
                line = sr.ReadLine();
            }
            return tempList;
        }

    }//end class
}//end namespace
