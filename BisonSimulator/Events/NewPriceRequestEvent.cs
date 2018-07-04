using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Pricing;
using System;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <summary>
    /// Simulates a price request from a customer.
    /// </summary>
    internal class NewPriceRequestEvent : ISimEvent
    {
        private readonly SimulationDependencyFactory _dependencyFactory;
        private readonly decimal? _amount;
        private readonly decimal? _value;
        private readonly BuySell _buySell;

        /// <inheritdoc />
        /// <summary>
        /// Date and time at which the event takes place.
        /// </summary>
        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulated price offer offered to customer.
        /// </summary>
        public Offer Offer { get; private set; }

        /// <summary>
        /// Simulates a price request from a customer.
        /// </summary>
        /// <param name="dependencyFactory"></param>
        /// <param name="requestAtTime">Date and time at which the event takes place.</param>
        /// <param name="requestAmount">Amount customer requests to trade. If not given a requestValue is required.</param>
        /// <param name="requestValue">Value customer requests to trade. If not given a requestAmount is required.</param>
        /// <param name="buySell">Is crypto-currency bought or sold.</param>
        public NewPriceRequestEvent(SimulationDependencyFactory dependencyFactory, DateTime requestAtTime, decimal? requestAmount, decimal? requestValue, BuySell buySell)
        {
            _dependencyFactory = dependencyFactory;
            _amount = requestAmount;
            _value = requestValue;
            _buySell = buySell;
            SimTime = requestAtTime;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates the event.
        /// </summary>
        public void Simulate()
        {
            switch (_buySell)
            {
                case BuySell.Buy:
                    Offer = _dependencyFactory.PricingEngine.GetBuyOffer(_amount, _value);
                    break;
                case BuySell.Sell:
                    Offer = _dependencyFactory.PricingEngine.GetSellOffer(_amount, _value);
                    break;
            }

            Offer.UserAccount = "SimAccount";
            Offer.UserId = 777;
            Offer.Currency = Currency.Eur;
            Offer.OpenTimeStamp = _dependencyFactory.DateTimeProvider.Now;
            Offer.Reference = Guid.NewGuid().ToString();
        }
    }
}
