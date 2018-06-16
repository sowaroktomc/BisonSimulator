using Sowalabs.Bison.Common.BisonApi;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulationDependencyFactory : Pricing.Dependencies.IDependencyFactory, Hedger.Dependencies.IDependencyFactory, LiquidityEngine.Dependencies.IDependencyFactory
    {
        public SimulatedMarketApi BitcoinMarketApi { get; }
        public SimulatedTimerFactory TimerFactory { get; }
        public IPricingService PricingService => PricingEngine;

        public Pricing.PricingEngine PricingEngine { get; }
        public Hedger.HedgingEngine HedgingEngine { get; }
        public LiquidityEngine.LiquidityEngine LiquidityEngine { get; }
        public SimulatedBankApi SolarisBank { get; }

        Pricing.Dependencies.IMarketApi Pricing.Dependencies.IDependencyFactory.GetMarketApi()
        {
            return BitcoinMarketApi;
        }

        public SimulationDependencyFactory(SimulationEngine engine)
        {
            BitcoinMarketApi = new SimulatedMarketApi {SubstractAmountsFromBooks = true};
            TimerFactory = new SimulatedTimerFactory(engine);
            PricingEngine = new Pricing.PricingEngine(this);
            HedgingEngine = new Hedger.HedgingEngine(this);
            LiquidityEngine = new LiquidityEngine.LiquidityEngine(this);

            SolarisBank = new SimulatedBankApi();
        }

        public Hedger.Dependencies.IMarketApi GetMarketApi()
        {
            return BitcoinMarketApi;
        }

        public LiquidityEngine.Dependencies.IBankApi GetBankApi(string bankSwiftCode)
        {
            return SolarisBank;
        }


        Common.Timer.ITimerFactory Hedger.Dependencies.IDependencyFactory.TimerFactory => TimerFactory;


    }
}
