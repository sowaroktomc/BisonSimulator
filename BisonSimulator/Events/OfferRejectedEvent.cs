using System;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Simulates customer rejecting offerd price offer.
    /// </summary>
    internal class OfferRejectedEvent : ISimEvent
    {
        private readonly NewPriceRequestEvent _priceRequestEvent;
        private readonly SimulationDependencyFactory _dependencyFactory;

        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulates customer rejecting offerd price offer.
        /// </summary>
        /// <param name="dependencyFactory"></param>
        /// <param name="priceRequestEvent">Simlated price request event offer from which the customer rejects.</param>
        /// <param name="rejectAtTime">Date and time the event takes place.</param>
        public OfferRejectedEvent(SimulationDependencyFactory dependencyFactory, NewPriceRequestEvent priceRequestEvent, DateTime rejectAtTime)
        {
            _priceRequestEvent = priceRequestEvent;
            _dependencyFactory = dependencyFactory;
            SimTime = rejectAtTime;
        }

        public void Simulate()
        {
            _dependencyFactory.PricingEngine.RejectOffer(this._priceRequestEvent.Offer.Id);
        }
    }
}
