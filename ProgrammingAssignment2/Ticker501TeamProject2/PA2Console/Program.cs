using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA2Console
{
    using Class_Library;
    class Program
    {
        static Account a;
        static Controller c;
        static void Main(string[] args)
        {
            c = new Controller(null);
            a = c.Account;
            Console.WriteLine("Welcome to Ticker501. Please select an option below:");
            while (true)
            {
                int menuChoice = ShowOptions();
                switch (menuChoice) //update if menu item is added
                {
                    case 1:
                        CreatePortfolio();
                        break;
                    case 2:
                        DeletePortfolio();
                        break;
                    case 3:
                        Deposit();
                        break;
                    case 4:
                        Withdraw();
                        break;
                    case 5:
                        AccountStats();
                        break;
                    case 6:
                        ViewPortfolio();
                        break;
                    case 7:
                        Volatility();
                        break;
                    case 8:
                        Exit();
                        return;
                        break;
                }
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void CreatePortfolio()
        {

        }

        static void DeletePortfolio()
        {

        }

        static void Deposit()
        {

        }

        static void Withdraw()
        {

        }
        
        static void AccountStats()
        {

        }

        static void ViewPortfolio()
        {

        }

        static void Volatility()
        {

        }

        static void Exit()
        {

        }

        static int ShowOptions()
        {
            Console.Clear();
            Console.WriteLine("You have $" + a.Funds + " available to you.");
            Console.WriteLine("Choose a number from the list below:");
            Console.WriteLine("\t1. Create Profile");
            Console.WriteLine("\t2. Delete Profile: Will sell all stocks in the profile and put money in your account");
            Console.WriteLine("\t3. Deposit money in account");
            Console.WriteLine("\t4. Withdraw money from account");
            Console.WriteLine("\t5. View Account statistics");
            Console.WriteLine("\t6. View Portfolio");
            Console.WriteLine("\t7. Simulate Volatility");
            Console.WriteLine("\t8. Exit Ticker501");
            Console.Write("Enter choice: ");
            while (true)
            {
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice < 1 || choice > 8) //Change if menu item is added
                    {
                        throw new Exception();
                    }
                    return choice;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Enter a number between 1 and 8 to continue.");
                    Console.Write("Enter choice: ");
                }
            }
        }
    }
}
