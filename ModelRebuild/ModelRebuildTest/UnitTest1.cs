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
            mm.Account.CreatePortfolio("CreatedPortOne", mm.VerifyStock);

            foreach(Stock stock in mm.Stocks)
            {
                Console.WriteLine(stock.ToString());
            }
            
            Assert.IsTrue(mm.Stocks.Count > 1);
            Assert.IsTrue(mm.Account.Portfolios.Count == 1);
        }

        [TestMethod]
        public void Test2()
        {
            MainModel mm = new MainModel();
            mm.LoadTickersFromFile();
            mm.Account.Deposit(1000);

            Assert.IsTrue(mm.Account.Funds == 1000 + Fee.DEPOSIT);
            Assert.IsTrue(mm.Account.Transactions.Count > 0);

            try
            {
                mm.Account.Withdraw(1000); //Throws exception
                Assert.Fail();
            }
            catch(ArgumentException e)
            {
                Assert.AreEqual("Withdrawing too much money", e.Message);
            }

            mm.Account.Withdraw(1000 + Fee.DEPOSIT * 2);
            Assert.AreEqual(0, Math.Round(mm.Account.Funds));

            Assert.IsTrue(mm.Account.Transactions.Count == 2);
            Assert.AreEqual(Fee.DEPOSIT * 2, mm.Account.CalculateGainLoss(mm.Stocks));
        }
    }
}
