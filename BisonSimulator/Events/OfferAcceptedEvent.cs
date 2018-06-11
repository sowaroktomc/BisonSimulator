using System;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Model;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OfferAcceptedEvent : ISimEvent
    {

        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;
        public User User { get; set; }

        public OfferAcceptedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime acceptAtTime)
        {
            this._priceRequestEvent = priceRequestEvent;
            this._dependencyFactory = dependencyFactory;
            this.SimTime = acceptAtTime;
            this.Id = Guid.NewGuid();
        }


        public Guid Id { get; }
        public DateTime SimTime { get; }
        public void Simulate()
        {
            this._dependencyFactory.PricingEngine.AcceptOffer(this._priceRequestEvent.Offer.Id);
            this._dependencyFactory.HedgingEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
            this._dependencyFactory.LiquidityEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
        }
    }
}
