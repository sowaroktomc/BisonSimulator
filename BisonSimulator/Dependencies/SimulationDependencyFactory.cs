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
        public SimulatedBankApi SolarisBank { get; }

        private readonly SimulationEngine _engine;
        public SimulatedDateTimeProvider DateTimeProvider { get; }


        /// <summary>
        /// Factory providing simulated dependency instances.
        /// </summary>
        public SimulationDependencyFactory(SimulationEngine engine)
        {
            _engine = engine;

            BitcoinMarketApi = new SimulatedMarketApi {SubstractAmountsFromBooks = true};
            TimerFactory = new SimulatedTimerFactory(_engine);
            PricingEngine = new Pricing.PricingEngine(this);
            HedgingEngine = new Hedger.HedgingEngine(this);
            DateTimeProvider = new SimulatedDateTimeProvider(_engine);

            SolarisBank = new SimulatedBankApi(_engine);
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

        LiquidityEngine.Dependencies.IResetEvent LiquidityEngine.Dependencies.IDependencyFactory.CreateResetEvent()
        {
            return new SimulatedResetEvent(_engine);
        }

        Common.DateTimeProvider.IDateTimeProvider LiquidityEngine.Dependencies.IDependencyFactory.DateTimeProvider => DateTimeProvider;
        Common.Timer.ITimerFactory LiquidityEngine.Dependencies.IDependencyFactory.TimerFactory => TimerFactory;


        #endregion

    }
}
