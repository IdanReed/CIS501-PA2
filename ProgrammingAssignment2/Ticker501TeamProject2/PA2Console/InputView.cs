using Class_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    class InputView
    {
        private InputHandler _inputHandler;
        private MenuHelper _mainMenu;
        private MenuHelper _portfolioMenu;


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
                .Add("Sell Stocks", null)
                .Add("View Portfolio Statistics", PortStats);
                
        }
        
        
        #region AccountLevel
        public void Start()
        {
            _mainMenu.ShowMenu(() => _inputHandler(new Event("accountBalance")));
        }

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
        
        private void AccountStats()
        {
            _inputHandler(new Event("accountStats"));
        }

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
        private void PortfolioMenu()
        {
            _portfolioMenu.ShowMenu();
        }

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

        private void PortSell()
        {
            
        }
        private void PortStats()
        {
            _inputHandler(new Event("portStats"));
            MenuHelper.PressEnter();
        }
        #endregion Portfolio Level


        private void Volatility()
        {
            //Should be good to go.
            //Event name is "simulate" and requires a string which will be either "high", "med", "low" and will return error if not.
            //The "simulate" event will broadcast a "showStocks" event which will print the updated stocks in the console.
            
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

        private void Exit()
        {
            _inputHandler(new Event("accountStats"));
            Environment.Exit(0);
        }

    }
}
