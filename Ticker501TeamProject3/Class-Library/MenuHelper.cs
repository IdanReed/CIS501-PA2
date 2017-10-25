using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library
{
    /// <summary>
    /// A menu helper class that allows you to add items to it and display them and get user input.
    /// It also contains some helper functions to make data validation less of a chore.
    /// 
    /// Reasoning:
    ///     I got really annoyed with typing the same validation stuff over and over again so I
    ///     consolidated it into some helpful functions. A lot of the ideas behind the .Then() method
    ///     and things like that came from my small experience with Rust. I liked the syntax a lot so I wanted
    ///     to try to implement something like it here.
    /// </summary>
    public class MenuHelper
    {
        private string _menuMessage;
        private List<MenuItem> _menuItems;

        public MenuHelper(string message)
        {
            _menuMessage = message;
            _menuItems = new List<MenuItem>();
        }

        /// <summary>
        /// Adds a new menu item to the list of items with the specified text and method
        /// </summary>
        /// <param name="text">The text to display on the menu</param>
        /// <param name="method">The method to run when selected</param>
        /// <returns>Itself to allow for chaining of .Add methods... Cuz it looks nice</returns>
        public MenuHelper Add(string text, Action method)
        {
            _menuItems.Add(new MenuItem(text, method));
            return this;
        }
        /// <summary>
        /// Shows the menu and prompts the user for an integer that needs to be
        /// an option shown. It then calls the method within the selected menuItem.
        /// </summary>
        public void ShowMenu()
        {
            bool doQuit;
            do
            {
                doQuit = true;
                PromptInt(
                    ToString(),
                    "Choice: ",
                    choice => choice < 1 || choice > _menuItems.Count,
                    "Please select a menu item shown"
                    )
                    .Then(value =>
                    {
                        _menuItems[value - 1].Call();
                        doQuit = false;
                    });
            } while (!doQuit);
            Console.Clear();
        }

        public void ShowMenu(Action a)
        {
            bool doQuit;
            do
            {
                a();
                doQuit = true;
                PromptInt(
                    ToString(),
                    "Choice: ",
                    choice => choice < 1 || choice > _menuItems.Count,
                    "Please select a menu item shown"
                    )
                    .Then(value =>
                    {
                        _menuItems[value - 1].Call();
                        doQuit = false;
                    });
            } while (!doQuit);
            Console.Clear();
        }

        /// <summary>
        /// Prompts the user for an integer with the question specified. It does all the data 
        /// validation required and doesn't return until the user provides a valid input.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<int> PromptInt(string mainQuestion, string promptText)
        {
            MenuReturn<int> result = new MenuReturn<int>(false, 0);
            bool canContinue = false;
            while (!canContinue)
            {
                Console.WriteLine("\n{0}", mainQuestion);
                Console.WriteLine("Type '!back' to go back");
                Console.Write(promptText);

                string input = Console.ReadLine();
                if (input == "!back")
                {
                    Console.Clear();
                    canContinue = true;
                }
                else
                {
                    try
                    {
                        int amount = int.Parse(input);
                        canContinue = true;
                        result = new MenuReturn<int>(true, amount);
                    }
                    catch
                    {
                        PrintError("Please enter an integer");
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Runs the prompt int method with the specified mainQuestion and promptText. It also takes one condition to be tested
        /// along side the regular validation. It will not leave while the condition is satisfied. If the condition is satisfied
        /// it prints the condition error.
        /// 
        /// Reasoning behind this:
        ///     I got really tired of data validation in my menu's so I started by creating the basic PromptInt, PromptDouble, etc.
        ///     Those worked great and made my menu's much simpler, but I noticed that with almost all my menus I was also testing
        ///     one additional condition that need to be satisfied and loop until its satisfied. So I added these methods to allow
        ///     for the conditions. It really cleaned up (I think) my menu page code.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <param name="condition">The condition to test against. If this condition is SATISFIED it will print an error, otherwise it returns the result</param>
        /// <param name="conditionError">The error to print when the condition is satisfied</param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        /// <example>
        /// This sample shows how to use the <see cref="PromptInt(string, string, Func{int, bool}, string)"/> method.
        /// <code>
        /// 
        /// MenuHelper.PromptInt(
        ///     "Please enter an integer between 1 and 6",
        ///     "Number:",
        ///     number=> number<1 || number> 6,
        ///     "Number must be between 1 and 6
        ///     ); 
        /// 
        /// </code>

        /// </example>
        public static MenuReturn<int> PromptInt(string mainQuestion, string promptText, Func<int, bool> condition, string conditionError)
        {
            bool canContinue = false;

            MenuReturn<int> result = new MenuReturn<int>(false, 0);
            while (!canContinue)
            {
                result = PromptInt(mainQuestion, promptText);
                if (result.Continue && condition(result.Value))
                {
                    PrintError(conditionError);
                }
                else //Exit
                {
                    canContinue = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Prompts the user for an double with the question specified. It does all the data 
        /// validation required and doesn't return until the user provides a valid input.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<double> PromptDouble(string mainQuestion, string promptText)
        {
            MenuReturn<double> result = new MenuReturn<double>(false, 0.0);
            bool canContinue = false;
            while (!canContinue)
            {
                Console.WriteLine("\n{0}", mainQuestion);
                Console.WriteLine("Type '!back' to go back");
                Console.Write(promptText);

                string input = Console.ReadLine();
                if (input == "!back")
                {
                    Console.Clear();
                    canContinue = true;
                }
                else
                {
                    try
                    {
                        double amount = double.Parse(input);
                        canContinue = true;
                        result = new MenuReturn<double>(true, amount);
                    }
                    catch
                    {
                        PrintError("Please enter a valid number");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Runs the prompt double method with the specified mainQuestion and promptText. It also takes one condition to be tested
        /// along side the regular validation. It will not leave while the condition is satisfied. If the condition is satisfied
        /// it prints the condition error.
        /// 
        /// Reasoning behind this:
        ///     I got really tired of data validation in my menu's so I started by creating the basic PromptInt, PromptDouble, etc.
        ///     Those worked great and made my menu's much simpler, but I noticed that with almost all my menus I was also testing
        ///     one additional condition that need to be satisfied and loop until its satisfied. So I added these methods to allow
        ///     for the conditions. It really cleaned up (I think) my menu page code.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <param name="condition">The condition to test against. If this condition is SATISFIED it will print an error, otherwise it returns the result</param>
        /// <param name="conditionError">The error to print when the condition is satisfied</param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<double> PromptDouble(string mainQuestion, string promptText, Func<double, bool> condition, string conditionError)
        {
            bool canContinue = false;

            MenuReturn<double> result = new MenuReturn<double>(false, 0.0);
            while (!canContinue)
            {
                result = PromptDouble(mainQuestion, promptText);
                if (result.Continue && condition(result.Value))
                {
                    PrintError(conditionError);
                }
                else //Exit
                {
                    canContinue = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Prompts the user for an string with the question specified. It does all the data 
        /// validation required and doesn't return until the user provides a valid input.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<string> PromptString(string mainQuestion, string promptText)
        {
            MenuReturn<string> result = new MenuReturn<string>(false, "");
            bool canContinue = false;
            while (!canContinue)
            {
                Console.WriteLine("\n{0}", mainQuestion);
                Console.WriteLine("Type '!back' to go back");
                Console.Write(promptText);

                string input = Console.ReadLine();
                if (input == "!back")
                {
                    Console.Clear();
                    canContinue = true;
                }
                else
                {
                    result = new MenuReturn<string>(true, input);
                    canContinue = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Runs the prompt string method with the specified mainQuestion and promptText. It also takes one condition to be tested
        /// along side the regular validation. It will not leave while the condition is satisfied. If the condition is satisfied
        /// it prints the condition error.
        /// 
        /// Reasoning behind this:
        ///     I got really tired of data validation in my menu's so I started by creating the basic PromptInt, PromptDouble, etc.
        ///     Those worked great and made my menu's much simpler, but I noticed that with almost all my menus I was also testing
        ///     one additional condition that need to be satisfied and loop until its satisfied. So I added these methods to allow
        ///     for the conditions. It really cleaned up (I think) my menu page code.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <param name="condition">The condition to test against. If this condition is SATISFIED it will print an error, otherwise it returns the result</param>
        /// <param name="conditionError">The error to print when the condition is satisfied</param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<string> PromptString(string mainQuestion, string promptText, Func<string, bool> condition, string conditionError)
        {
            bool canContinue = false;

            MenuReturn<string> result = new MenuReturn<string>(false, "");
            while (!canContinue)
            {
                result = PromptString(mainQuestion, promptText);
                if (result.Continue && condition(result.Value))
                {
                    PrintError(conditionError);
                }
                else //Exit
                {
                    canContinue = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Prompts the user for an char with the question specified. It does all the data 
        /// validation required and doesn't return until the user provides a valid input.
        /// </summary>
        /// <param name="mainQuestion">The main question to ask</param>
        /// <param name="promptText">The string to prompt the user with. eg. Amount: </param>
        /// <returns>A MenuReturn with the entered value stored as the value</returns>
        public static MenuReturn<char> PromptChar(string mainQuestion, string promptText)
        {
            bool canContinue = false;

            MenuReturn<char> result = new MenuReturn<char>(false, ' ');
            while (!canContinue)
            {
                Console.WriteLine("\n{0}", mainQuestion);
                Console.WriteLine("Type '!back' to go back");
                Console.Write(promptText);

                string input = Console.ReadLine();
                if (input == "!back")
                {
                    Console.Clear();
                    canContinue = true;
                }
                else
                {
                    try
                    {
                        char choice = input.ToLower()[0];
                        canContinue = true;
                        result = new MenuReturn<char>(true, choice);
                    }
                    catch
                    {
                        PrintError("Please enter a character");
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Prints a nice looking error message centered at the top of the screen
        /// </summary>
        /// <param name="err">The error to print</param>
        public static void PrintError(string err)
        {
            Console.Clear();
            string errHead = "---------------------------------------ERROR---------------------------------------";
            string errFoot = "-----------------------------------------------------------------------------------";

            //Center the printing
            Console.SetCursorPosition((Console.WindowWidth - errHead.Length) / 2, Console.CursorTop);
            Console.WriteLine(errHead);
            Console.SetCursorPosition((Console.WindowWidth - err.Length) / 2, Console.CursorTop);
            Console.WriteLine(err);
            Console.SetCursorPosition((Console.WindowWidth - errFoot.Length) / 2, Console.CursorTop);
            Console.WriteLine(errFoot);
        }

        /// <summary>
        /// Prompts the user to press enter then clears the screen
        /// </summary>
        public static void PressEnter()
        {
            Console.WriteLine("\n\nPress enter to continue");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Returns all the menuItems in a nice looking list
        /// </summary>
        /// <returns>A string containing all the menuItems in a nice looking list</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_menuMessage);
            for (int i = 0; i < _menuItems.Count; i++)
            {
                sb.AppendFormat("\n\t{0}) {1}", i + 1, _menuItems[i]);
            }
            return sb.ToString();

        }
    }

    /// <summary>
    /// The return type for all the PromptInt, PromptDouble, etc. The Continue
    /// value tells if the user wants to go back or not. If false then the user
    /// selected the !back options, otherwise they want to continue. The value
    /// is whatever they entered, if anything.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct MenuReturn<T>
    {
        public readonly bool Continue;
        public readonly T Value;

        /// <summary>
        /// The main constructor for MenuReturn
        /// </summary>
        /// <param name="cont">Whether or not the user wants to continue or not</param>
        /// <param name="val">The value (if any) the user entered in</param>
        public MenuReturn(bool cont, T val)
        {
            Continue = cont;
            Value = val;
        }

        /// <summary>
        /// If the user wants to continue and entered a value, it runs the specified function
        /// and passes in the entered value.
        /// 
        /// Reasoning:
        ///     Pretty much all cosmetic. Every time I called a MenuHelper.Prompt method
        ///     I was having to check if the user wanted to continue, ex:
        ///     
        ///         MenuResult res = MenuHelper.PromptInt(...);
        ///         if(result.Continue){
        ///             ...
        ///         }
        ///         
        ///     I didn't like that and it annoyed me so this method makes it nicer (I think at least, some people may not like it)
        ///     to use. New usage:
        ///     
        ///         MenuHelper.PromptInt(...).Then(val =>{
        ///             ...
        ///         });
        /// </summary>
        /// <param name="func">The function to run if the user wished to continue</param>
        public void Then(Action<T> func)
        {
            if (Continue)
            {
                func(Value);
            }
        }
    }


    /// <summary>
    /// This simple class is just added to the list of menuItems and
    /// gets displayed when ShowMenu is called. Probably could've just
    /// stored these in a Tuple or something but I didn't know if I was
    /// gonna want some more methods or anything.
    /// </summary>
    public class MenuItem
    {
        private string _text;
        private Action _method;


        /// <summary>
        /// The main consturctor for a MenuItem
        /// </summary>
        /// <param name="text">The text to display on the menu</param>
        /// <param name="method">The method to run when the item is selected</param>
        public MenuItem(string text, Action method)
        {
            _text = text;
            _method = method;
        }

        /// <summary>
        /// Returns the text to display on the menu
        /// </summary>
        /// <returns>The text to display on the menu</returns>
        public override string ToString()
        {
            return _text;
        }

        /// <summary>
        /// Calls the method that was provided in the constructor
        /// </summary>
        public void Call()
        {
            _method();
        }
    }
}
