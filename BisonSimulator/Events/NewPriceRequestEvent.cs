using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Pricing;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class NewPriceRequestEvent : ISimEvent
    {

        private readonly PricingEngine _pricingEngine;
        private readonly decimal _volume;
        private readonly BuySell _buySell;

        public Guid Id { get; }
        public DateTime SimTime { get; }

        public Offer Offer { get; private set; }

        public NewPriceRequestEvent(PricingEngine pricingEngine, DateTime requestAtTime, decimal requestVolume, BuySell buySell)
        {
            this.Id = Guid.NewGuid();
            this._pricingEngine = pricingEngine;
            this._volume = requestVolume;
            this._buySell = buySell;
            this.SimTime = requestAtTime;
        }

        public void Simulate()
        {
            switch (this._buySell)
            {
                case BuySell.Buy:
                    this.Offer = this._pricingEngine.GetBuyOffer(this._volume);
                    break;
                case BuySell.Sell:
                    this.Offer = this._pricingEngine.GetSellOffer(this._volume);
                    break;
            }
        }
    }
}
