using Sowalabs.Bison.ProfitSim.Dependencies;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OfferAcceptedEvent : ISimEvent
    {

        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;
        private DateTime _simTime;

        public DateTime SimTime { get { return _simTime; } set { _simTime = value; } }

        public OfferAcceptedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime acceptAtTime)
        {
            this._priceRequestEvent = priceRequestEvent;
            this._dependencyFactory = dependencyFactory;
            this._simTime = acceptAtTime;
        }
        public void Simulate()
        {
            this._dependencyFactory.PricingEngine.AcceptOffer(this._priceRequestEvent.Offer.Id);
            this._dependencyFactory.HedgingEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
            this._dependencyFactory.LiquidityEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
        }
    }
}
