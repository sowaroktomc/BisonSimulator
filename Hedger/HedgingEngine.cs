using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Hedger.Dependencies;
using Sowalabs.Bison.Hedger.ExecutionStrategies;
using Sowalabs.Bison.Hedger.WhenStrategies;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sowalabs.Bison.Hedger
{
    public class HedgingEngine
    {
        private readonly IDependencyFactory _dependencyFactory;
        private readonly List<IWhenStrategy> _whenStrategies = new List<IWhenStrategy>();
        public IExecutionStrategy ExecutionStrategy { get; private set; }
        public DelayExecutionStrategy WhenStrategy { get; private set; }

        public IReadOnlyCollection<IWhenStrategy> WhenStrategies => new ReadOnlyCollection<IWhenStrategy>(this._whenStrategies);

        private readonly List<Common.Trading.Offer> _offers;

        public HedgingEngine() : this(new HedgerDependencyFactory())
        {
        }

        public HedgingEngine(IDependencyFactory dependencyFactory)
        {
            this._offers = new List<Offer>();
            this._dependencyFactory = dependencyFactory;

            this.ExecutionStrategy = new SimpleExecutionStrategy(dependencyFactory);
            this.WhenStrategy = new DelayExecutionStrategy(this, dependencyFactory);
            this._whenStrategies.Add(this.WhenStrategy);
        }

        public void RegisterAcceptedOffer(Common.Trading.Offer offer)
        {
            lock (this._offers)
            {
                this._offers.Add(offer);
            }

            this._whenStrategies.ForEach(strategy => strategy.RegisterAcceptedOffer(offer));

        }

        public void ExecuteOffers()
        {
            List<Offer> offers;
            lock (this._offers)
            {
                offers = new List<Offer>(this._offers);
                this._offers.Clear();
            }

            this.ExecutionStrategy.ExecuteOffers(offers);
        }

    }
}
