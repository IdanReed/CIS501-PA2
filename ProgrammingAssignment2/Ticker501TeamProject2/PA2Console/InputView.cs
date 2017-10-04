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
                .Add("Simulate Volatility", Volatility);
        }
        
        
        public void Start()
        {
            _mainMenu.ShowMenu();
        }
        private void CreatePortfolio()
        {

        }

        private void DeletePortfolio()
        {

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

        }

        private void Volatility()
        {

        }

    }
}
