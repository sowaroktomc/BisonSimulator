using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;
using System;
using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class User
    {
        public BuySell BuySell { get; set; }
        public decimal Volume { get; set; }
        public bool AcceptsOffer { get; set; }
        public int RequestDelay { get; set; }
        public int AcceptRejectDelay { get; set; }

        public User(BuySell buySell, decimal volume)
        {
            this.BuySell = buySell;
            this.Volume = volume;
        }

        public void CreateEvents(SimulationDependencyFactory dependencyFactory, DateTime simStart)
        {

            var priceRequest = new NewPriceRequestEvent(dependencyFactory.PricingEngine, simStart.AddSeconds(this.RequestDelay), this.Volume, this.BuySell);
            SimulationEngine.Instance.AddEvent(priceRequest);

            var acceptRejectDelay = simStart.AddSeconds(this.RequestDelay + this.AcceptRejectDelay);
            if (this.AcceptsOffer)
            {
                SimulationEngine.Instance.AddEvent(new OfferAcceptedEvent(dependencyFactory, priceRequest, acceptRejectDelay));
            }
            else
            {
                SimulationEngine.Instance.AddEvent(new OfferRejectedEvent(dependencyFactory, priceRequest, acceptRejectDelay));
            }

        }

    }
}
