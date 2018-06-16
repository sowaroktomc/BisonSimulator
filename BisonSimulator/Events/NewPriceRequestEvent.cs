using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Pricing;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class NewPriceRequestEvent : ISimEvent
    {
        private readonly PricingEngine _pricingEngine;
        private readonly decimal? _amount;
        private readonly decimal? _value;
        private readonly BuySell _buySell;

        public Guid Id { get; }
        public DateTime SimTime { get; }

        public Offer Offer { get; private set; }

        public NewPriceRequestEvent(PricingEngine pricingEngine, DateTime requestAtTime, decimal? requestAmount, decimal? requestValue, BuySell buySell)
        {
            Id = Guid.NewGuid();
            _pricingEngine = pricingEngine;
            _amount = requestAmount;
            _value = requestValue;
            _buySell = buySell;
            SimTime = requestAtTime;
        }

        public void Simulate()
        {
            switch (_buySell)
            {
                case BuySell.Buy:
                    Offer = _pricingEngine.GetBuyOffer(_amount, _value);
                    break;
                case BuySell.Sell:
                    Offer = _pricingEngine.GetSellOffer(_amount, _value);
                    break;
            }
        }
    }
}
