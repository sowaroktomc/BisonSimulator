using System;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OfferRejectedEvent : ISimEvent
    {
        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;

        public OfferRejectedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime rejectAtTime)
        {
            this._priceRequestEvent = priceRequestEvent;
            this._dependencyFactory = dependencyFactory;
            this.SimTime = rejectAtTime;
            this.Id = Guid.NewGuid();
        }


        public Guid Id { get; }
        public DateTime SimTime { get; }
        public void Simulate()
        {
            this._dependencyFactory.PricingEngine.RejectOffer(this._priceRequestEvent.Offer.Id);
        }
    }
}
