using Sowalabs.Bison.ProfitSim.Dependencies;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Simulates customer accepting offerd price offer.
    /// </summary>
    internal class OfferAcceptedEvent : ISimEvent
    {

        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;

        /// <summary>
        /// Date and time at which the event takes place.
        /// </summary>
        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulates customer accepting offerd price offer.
        /// </summary>
        /// <param name="dependencyFactory"></param>
        /// <param name="priceRequestEvent">Simlated price request event offer from which the customer accepts.</param>
        /// <param name="acceptAtTime">Date and time the event takes place.</param>
        public OfferAcceptedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime acceptAtTime)
        {
            _priceRequestEvent = priceRequestEvent;
            _dependencyFactory = dependencyFactory;
            SimTime = acceptAtTime;
        }

        public void Simulate()
        {
            _dependencyFactory.PricingEngine.AcceptOffer(this._priceRequestEvent.Offer.Id);
            _dependencyFactory.HedgingEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
            _dependencyFactory.LiquidityEngine.RegisterAcceptedOffer(this._priceRequestEvent.Offer);
        }
    }
}
