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
            return this.BitcoinMarketApi;
        }

        public SimulationDependencyFactory()
        {
            this.BitcoinMarketApi = new SimulatedMarketApi();
            this.TimerFactory = new SimulatedTimerFactory();
            this.PricingEngine = new Pricing.PricingEngine(this);
            this.HedgingEngine = new Hedger.HedgingEngine(this);
            this.LiquidityEngine = new LiquidityEngine.LiquidityEngine(this);

            this.SolarisBank = new SimulatedBankApi();
        }

        public Hedger.Dependencies.IMarketApi GetMarketApi()
        {
            return this.BitcoinMarketApi;
        }

        public LiquidityEngine.Dependencies.IBankApi GetBankApi(string bankSwiftCode)
        {
            return this.SolarisBank;
        }


        Common.Timer.ITimerFactory Hedger.Dependencies.IDependencyFactory.TimerFactory => this.TimerFactory;


    }
}
