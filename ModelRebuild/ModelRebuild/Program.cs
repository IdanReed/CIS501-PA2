using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    class Program
    {
        static void Main(string[] args)
        {
            MainModel mm = new MainModel();
            mm.LoadTickersFromFile();
            mm.account.CreatePortfolio("CreatedPortOne");
            
            mm.account.portfolios[0].
            Console.ReadLine();
        }
    }
}
