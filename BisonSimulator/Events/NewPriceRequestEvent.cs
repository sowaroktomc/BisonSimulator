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
        private DateTime _simTime;

        public DateTime SimTime { get { return _simTime; } set { _simTime = value; } }

        public Offer Offer { get; private set; }

        public NewPriceRequestEvent(PricingEngine pricingEngine, DateTime requestAtTime, decimal? requestAmount, decimal? requestValue, BuySell buySell)
        {
            _pricingEngine = pricingEngine;
            _amount = requestAmount;
            _value = requestValue;
            _buySell = buySell;
            _simTime = requestAtTime;
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
