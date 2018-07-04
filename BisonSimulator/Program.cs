using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using System.Net;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 5;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MultiProfitSimForm());
            //return;
            //Application.Run(new CleanupForm());

            var simEngine = new SimulationEngine(DateTime.MinValue);
            var dependencyFactory = new SimulationDependencyFactory(simEngine);
            var liquidityEngine = new LiquidityEngine.LiquidityEngine(dependencyFactory);
            //LiquidityEngineMoq.SetSingleton(new LiquidityEngineMoq(dependencyFactory));
            dependencyFactory.SolarisBank.SetAccountBalance("TestUserAccount", 50000);
            dependencyFactory.SolarisBank.TransferDelay = 4;

            simEngine.AfterEventSimulation += (sender, args) =>
            {
                if (!liquidityEngine.IsDisposed && simEngine.PeekLastEvent().SimTime.Minute > 0)
                {
                    liquidityEngine.Dispose();
                }
            };


            liquidityEngine.Initialize();


            liquidityEngine.RegisterAcceptedOffer(new Offer()
            {
                Amount = 2.3m,
                BuySell = BuySell.Buy,
                Currency = Currency.Eur,
                Price = 601.25m,
                Reference = "RokTomc5",
                UserId = 45,
                UserAccount = "TestUserAccount",
                OpenTimeStamp = DateTime.Now
            });
            liquidityEngine.RegisterAcceptedOffer(new Offer()
            {
                Amount = 1.3m,
                BuySell = BuySell.Sell,
                Currency = Currency.Eur,
                Price = 601.25m,
                Reference = "RokTomc6",
                UserId = 45,
                UserAccount = "TestUserAccount",
                OpenTimeStamp = DateTime.Now.AddSeconds(1)
            });
            liquidityEngine.RegisterAcceptedOffer(new Offer()
            {
                Amount = 1m,
                BuySell = BuySell.Buy,
                Currency = Currency.Eur,
                Price = 600m,
                Reference = "RokTomc7",
                UserId = 45,
                UserAccount = "TestUserAccount",
                OpenTimeStamp = DateTime.Now.AddSeconds(2)
            });
            //LiquidityEngine.LiquidityEngine.Instance.Dispose();
            simEngine.Simulate();

        }
    }
}
