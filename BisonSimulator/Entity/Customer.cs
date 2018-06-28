using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Entity
{
    /// <summary>
    /// Simulated customer description.
    /// </summary>
    internal class Customer
    {
        /// <summary>
        /// Is customer buying or selling cryptos.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public BuySell BuySell { get; set; }

        /// <summary>
        /// Amount of crypto-currency customer is trading. If null then a Value field is required.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Value of crypto-currency customer is trading. If null then an Amount field is required.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Does the customer accept given price offer.
        /// </summary>
        public bool AcceptsOffer { get; set; }

        /// <summary>
        /// Number of seconds after simulation started when the customer request a price offer.
        /// </summary>
        public int RequestDelay { get; set; }

        /// <summary>
        /// Number of seconds after being given a price offer the customer accepts or reject offer.
        /// </summary>
        public int AcceptRejectDelay { get; set; }

        /// <summary>
        /// Is customer a market customer (not Bison customer). Market customers just drain the order book but do not affect profit / loss.
        /// </summary>
        public bool IsMarketTrader { get; set; }

        /// <summary>
        /// Simulated customer description.
        /// </summary>
        /// <param name="buySell">Is customer buying or selling cryptos.</param>
        /// <param name="amount">Amount of crypto-currency customer is trading. If null then a Value field is required.</param>
        /// <param name="value">Value of crypto-currency customer is trading. If null then an Amount field is required.</param>
        public Customer(BuySell buySell, decimal? amount, decimal? value)
        {
            if (!amount.HasValue && !value.HasValue)
            {
                throw new ArgumentNullException(nameof(amount));
            }

            BuySell = buySell;
            Amount = amount;
            Value = value;
        }

        /// <summary>
        /// Create a list of simulation events which simulate customer's behaviour.
        /// </summary>
        /// <param name="dependencyFactory">Dependency factory to be used during events creation.</param>
        /// <param name="simStart">At what time does the current simulated time period start.</param>
        /// <returns>List of simulation events which simulate customer's behaviour.</returns>
        public List<ISimEvent> CreateEvents(SimulationDependencyFactory dependencyFactory, DateTime simStart)
        {
            var events = new List<ISimEvent>();

            if (IsMarketTrader)
            {
                events.Add(new MarketTradeEvent(dependencyFactory, simStart.AddSeconds(RequestDelay), Amount, Value, BuySell));
                return events;
            }

            var priceRequest = new NewPriceRequestEvent(dependencyFactory, simStart.AddSeconds(RequestDelay), Amount, Value, BuySell);
            events.Add(priceRequest);

            var acceptRejectDelay = simStart.AddSeconds(RequestDelay + AcceptRejectDelay);
            if (AcceptsOffer)
            {
                events.Add(new OfferAcceptedEvent(dependencyFactory, priceRequest, acceptRejectDelay));
            }
            else
            {
                events.Add(new OfferRejectedEvent(dependencyFactory, priceRequest, acceptRejectDelay));
            }

            return events;
        }

        /// <summary>
        /// Returns clone of current instance.
        /// </summary>
        /// <returns>Clone of current instance.</returns>
        public Customer Clone()
        {
            return new Customer(BuySell, Amount, Value)
            {
                AcceptsOffer = AcceptsOffer,
                RequestDelay = RequestDelay,
                AcceptRejectDelay = AcceptRejectDelay,
                IsMarketTrader = IsMarketTrader
            };
        }

    }
}
