using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelRebuild;
using System.Diagnostics;

namespace ModelRebuildTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainModel mm = new MainModel();
            mm.LoadTickersFromFile();
            mm.account.CreatePortfolio("CreatedPortOne");

            foreach(Stock stock in mm.stocks)
            {
                Console.WriteLine(stock.ToString());
            }
            
            Debug.Assert(mm.stocks.Count > 1);
            Debug.Assert(mm.account.portfolios.Count == 1);
        }
    }
}
