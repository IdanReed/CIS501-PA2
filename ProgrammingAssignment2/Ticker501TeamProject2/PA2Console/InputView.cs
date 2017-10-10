using Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    /// <summary>
    /// The inputView for the console app. Handles all console input
    /// </summary>
    class InputView
    {
        /// <summary>
        /// The InputHandler to pass all the events to
        /// </summary>
        private InputHandler _inputHandler;
        /// <summary>
        /// The Main menu to display on startup
        /// </summary>
        private MenuHelper _mainMenu;
        /// <summary>
        /// The portfolio menu to display when viewing a portfolio
        /// </summary>
        private MenuHelper _portfolioMenu;


        /// <summary>
        /// The main constructor which takes an InputHandler and creates the menus
        /// </summary>
        /// <param name="han">The inputhandler to used</param>
        public InputView(InputHandler han)
        {
            _inputHandler = han;
            _mainMenu = new MenuHelper("Welcome to Ticker501. Please select an option below:");
            _mainMenu
                .Add("Create Profile", CreatePortfolio)
                .Add("Delete Profile", DeletePortfolio)
                .Add("Deposit Money", Deposit)
                .Add("Withdraw Money", Withdraw)
                .Add("View Account statistics", AccountStats)
                .Add("View a Portfolio", ViewPortfolio)
                .Add("Simulate Volatility", Volatility)
                .Add("Exit", Exit);

            _portfolioMenu = new MenuHelper("Please select an option below:");
            _portfolioMenu
                .Add("Buy Stocks", PortBuy)
                .Add("Sell Stocks", PortSell)
                .Add("View Portfolio Statistics", PortStats);
                
        }
        
        
        #region AccountLevel
        /// <summary>
        /// Starts the MainMenu;
        /// </summary>
        public void Start()
        {
            _mainMenu.ShowMenu(() => _inputHandler(new Event("accountBalance")));
        }

        /// <summary>
        /// Prompts the user for the name of a new portfolio and calls the "newPort" event
        /// </summary>
        private void CreatePortfolio()
        {
            bool canContinue;
            do
            {
                canContinue = true;
                MenuHelper.PromptString(
                    "Enter the name of the new portfolio",
                    "Name: "
                    ).Then(name =>
                    {
                        Event e = new Event(name, "newPort");

                        _inputHandler(e).Catch(message =>
                        {
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);
            Console.WriteLine("The portfolio has been created and you can view it through the view portfolio option.");
            MenuHelper.PressEnter();
        }

        /// <summary>
        /// Prompts the user for the name of a portfolio to delete and calls the "deletePort" event
        /// </summary>
        private void DeletePortfolio()
        {
            bool canContinue;
            do
            {
                canContinue = true;
                MenuHelper.PromptString(
                    "Enter the name of the portfolio you'd like to delete", 
                    "Name: "
                    ).Then(name =>
                    {
                        Event e = new Event(name, "deletePort");

                        _inputHandler(e).Catch(message =>
                        {
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);
            Console.WriteLine("The portfolio has been deleted and all the stocks have been sold.");
            MenuHelper.PressEnter();
        }

        /// <summary>
        /// Prompts the user for an amount to deposit and calls the "deposit" event
        /// </summary>
        private void Deposit()
        {
            bool canContinue;
            do
            {
                canContinue = true;
                //Call our menu helper to prompt a double with the given text
                MenuHelper.PromptDouble(
                    "Enter amount to deposit",
                    "Amount $:"
                    ).Then(depositAmount => //Then menu helper stores the entered variable into "depositAmount" and runs the provided function
                    {
                        //Create a new event and store the depositAmount into it
                        Event e = new Event(depositAmount, "deposit");
                        //Send that event to the InputHandler and catch it's error with the provided function
                        _inputHandler(e).Catch(message =>
                        {
                            //Print the message returned by the controller
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);
            Console.WriteLine("Deposit Complete.");
            MenuHelper.PressEnter();
        }

        /// <summary>
        /// Prompts the user for an amount to withdraw and calls the "withdraw" event
        /// </summary>
        private void Withdraw()
        {
            bool canContinue; 
            do
            {
                canContinue = true;
                //Call our menu helper to prompt a double with the given text
                MenuHelper.PromptDouble(
                    "Enter amount to withdraw",
                    "Amount $:"
                    ).Then(withdrawAmount => //Then menu helper stores the entered variable into "withdrawAmount" and runs this function
                    {
                        //Create a new event and store the withdrawAmount into it
                        Event e = new Event(withdrawAmount, "withdraw");
                        //Send that event to the InputHandler and catch it's error with this function
                        _inputHandler(e).Catch(message =>
                        {
                            //Print the message returned by controller
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);
            Console.WriteLine("Withdraw Complete.");
            MenuHelper.PressEnter();
        }
        
        /// <summary>
        /// Calls the "accountStats" event
        /// </summary>
        private void AccountStats()
        {
            _inputHandler(new Event("accountStats"));
        }

        /// <summary>
        /// Prompts the user for the name of a portfolio to view and calls the portView event
        /// </summary>
        private void ViewPortfolio()
        {
            _inputHandler(new Event("showPortfolios"));
            bool canContinue;
            do
            {
                canContinue = true;
                MenuHelper.PromptString(
                    "Enter the name of the portfolio to view",
                    "Name: "
                    ).Then(name =>
                    {
                        Event e = new Event(name, "portView");

                        _inputHandler(e).Catch(message =>
                        {
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);

            MenuHelper.PressEnter();
            PortfolioMenu();
        }
        #endregion Account Level

        #region Portfolio Level
        /// <summary>
        /// Shows the portfolio menu
        /// </summary>
        private void PortfolioMenu()
        {
            _portfolioMenu.ShowMenu();
        }

        /// <summary>
        /// Prompts the user for the abbreviation of the stock to buy and then their method of purchase and then
        /// the amount for their purchase. Calls either the "portBuyShares" or the "portBuyCost" depending on what they select.
        /// </summary>
        private void PortBuy()
        {
            _inputHandler(new Event("showStocks"));

            
            bool canContinue;
            do
            {
                canContinue = true;
                MenuHelper.PromptString(
                    "Enter the abbreviation of the stock you'd like to buy",
                    "Abbreviation: "
                    ).Then(abbreviation =>
                    {
                        MenuHelper.PromptInt(
                            "Please select your method of purchase: \n\t1) Number of stocks to purchace\n\t2) Amount in dollars",
                            "Choice: ",
                            choice => choice != 1 && choice != 2,
                            "Please enter a choice shown below"
                            ).Then(choice =>
                            {
                                if(choice == 1)
                                {
                                    MenuHelper.PromptInt(
                                        "How many stocks would you like to buy of '" + abbreviation + "'?",
                                        "Amount: "
                                        ).Then(amt =>
                                        {
                                            Event e = new Event(Tuple.Create(abbreviation, amt), "portBuyShares");
                                            _inputHandler(e).Catch(message =>
                                            {
                                                MenuHelper.PrintError(message);
                                                canContinue = false;
                                            });
                                        });
                                }
                                else
                                {
                                    MenuHelper.PromptDouble(
                                        "How much money would you like to spend on '" + abbreviation + "'?",
                                        "Amount: "
                                        ).Then(cost =>
                                        {
                                            Event e = new Event(Tuple.Create(abbreviation, cost), "portBuyCost");
                                            _inputHandler(e).Catch(message =>
                                            {
                                                MenuHelper.PrintError(message);
                                                canContinue = false;
                                            });
                                        });
                                }
                            });
                    });
            }
            while (!canContinue);

            Console.WriteLine("Successfully purchased stocks, view portfolio stats to see them");
            MenuHelper.PressEnter();
        }

        /// <summary>
        /// Prompts the user for the abbreviation of the stock they's like to sell and the amount then
        /// calls the "portSell" event.
        /// </summary>
        private void PortSell()
        {
            bool canContinue;
            do
            {
                canContinue = true;
                _inputHandler(new Event("showPortStocks"));
                MenuHelper.PromptString(
                    "Enter the abbreviation of the stock you'd like to sell",
                    "Abbreviation: "
                    ).Then(abbreviation =>
                    {
                        MenuHelper.PromptInt(
                            "Enter the number of stocks you'd like to sell of " + abbreviation,
                            "Amount: "
                            ).Then(amount =>
                            {
                                _inputHandler(new Event(Tuple.Create(abbreviation, amount), "portSell")).Catch(e =>
                                {
                                    MenuHelper.PrintError(e);
                                    canContinue = false;
                                });
                            });
                    });

            } while (!canContinue);
        }
        /// <summary>
        /// Calls the "portStats" event
        /// </summary>
        private void PortStats()
        {
            _inputHandler(new Event("portStats"));
            MenuHelper.PressEnter();
        }
        #endregion Portfolio Level


        /// <summary>
        /// Prompts the user for the volitility and then calls "simulate"
        /// </summary>
        private void Volatility()
        {
            
            bool canContinue;
            do
            {
                canContinue = true;
                MenuHelper.PromptString(
                    "Enter the volatility: (high, med, low)",
                    "Volatility: "
                    ).Then(vol =>
                    {
                        Event e = new Event(vol, "simulate");

                        _inputHandler(e).Catch(message =>
                        {
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);

            MenuHelper.PressEnter();
        }

        /// <summary>
        /// Calls "deleteAllPortfolios" and "accountStats" events and then exits.
        /// </summary>
        private void Exit()
        {
            _inputHandler(new Event("deleteAllPortfolios"));
            _inputHandler(new Event("accountStats"));
            Environment.Exit(0);
        }

    }
}
