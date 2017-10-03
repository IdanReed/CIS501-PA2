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
                MenuHelper.PromptDouble(
                    "Enter amount to deposit",
                    "Amount $:"
                    ).Then(depositAmount =>
                    {
                        Event e = new Event(depositAmount, "deposit");
                        _inputHandler(e).Catch(message =>
                        {
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
                MenuHelper.PromptDouble(
                    "Enter amount to withdraw",
                    "Amount $:"
                    ).Then(withdrawAmount =>
                    {
                        Event e = new Event(withdrawAmount, "withdraw");
                        _inputHandler(e).Catch(message =>
                        {
                            MenuHelper.PrintError(message);
                            canContinue = false;
                        });
                    });
            }
            while (!canContinue);
            Console.WriteLine("Withdraw Complete.");
            MenuHelper.PressEnter();
            /*double amtToWithdraw = -1;
            while (amtToWithdraw == -1)
            {
                Console.Write("Enter amount to withdraw (a fee of $" + c.DepositFee + " will be assessed): $");
                try
                {
                    amtToWithdraw = Convert.ToDouble(Console.ReadLine());
                    if (amtToWithdraw - c.DepositFee > a.Funds)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please make sure you have enough funds.");
                    amtToWithdraw = -1;
                }
            }
            c.Withdraw(amtToWithdraw);
            Console.WriteLine("Withdrawl Complete.");*/
        }
        
        private void AccountStats()
        {

        }

        private void ViewPortfolio()
        {

        }

        private void Volatility()
        {

        }

    }
}
