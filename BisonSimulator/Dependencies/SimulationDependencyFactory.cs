using Sowalabs.Bison.Common.BisonApi;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    /// <summary>
    /// Factory providing simulated dependency instances.
    /// </summary>
    internal class SimulationDependencyFactory : Pricing.Dependencies.IDependencyFactory, Hedger.Dependencies.IDependencyFactory, LiquidityEngine.Dependencies.IDependencyFactory
    {
        public SimulatedMarketApi BitcoinMarketApi { get; }
        public SimulatedTimerFactory TimerFactory { get; }
        public Pricing.PricingEngine PricingEngine { get; }
        public Hedger.HedgingEngine HedgingEngine { get; }
        public LiquidityEngine.LiquidityEngine LiquidityEngine { get; }
        public SimulatedBankApi SolarisBank { get; }


        /// <summary>
        /// Factory providing simulated dependency instances.
        /// </summary>
        public SimulationDependencyFactory(SimulationEngine engine)
        {
            BitcoinMarketApi = new SimulatedMarketApi {SubstractAmountsFromBooks = true};
            TimerFactory = new SimulatedTimerFactory(engine);
            PricingEngine = new Pricing.PricingEngine(this);
            HedgingEngine = new Hedger.HedgingEngine(this);
            LiquidityEngine = new LiquidityEngine.LiquidityEngine(this);

            SolarisBank = new SimulatedBankApi();
        }

        #region Hedger

        IPricingService Hedger.Dependencies.IDependencyFactory.PricingService => PricingEngine;

        Common.Timer.ITimerFactory Hedger.Dependencies.IDependencyFactory.TimerFactory => TimerFactory;

        public Hedger.Dependencies.IMarketApi GetMarketApi()
        {
            return BitcoinMarketApi;
        }

        #endregion

        #region Pricing

        Pricing.Dependencies.IMarketApi Pricing.Dependencies.IDependencyFactory.GetMarketApi()
        {
            return BitcoinMarketApi;
        }

        #endregion

        #region Liquidity

        LiquidityEngine.Dependencies.IBankApi LiquidityEngine.Dependencies.IDependencyFactory.GetBankApi(string bankSwiftCode)
        {
            return SolarisBank;
        }

        #endregion

    }
}
