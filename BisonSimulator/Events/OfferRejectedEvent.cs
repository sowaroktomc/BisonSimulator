using System;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OfferRejectedEvent : ISimEvent
    {
        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;
        private DateTime _simTime;

        public DateTime SimTime { get { return _simTime; } set { _simTime = value; } }

        public OfferRejectedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime rejectAtTime)
        {
            this._priceRequestEvent = priceRequestEvent;
            this._dependencyFactory = dependencyFactory;
            this._simTime = rejectAtTime;
        }

        public void Simulate()
        {
            this._dependencyFactory.PricingEngine.RejectOffer(this._priceRequestEvent.Offer.Id);
        }
    }
}
