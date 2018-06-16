using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class Customer
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public BuySell BuySell { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Value { get; set; }
        public bool AcceptsOffer { get; set; }
        public int RequestDelay { get; set; }
        public int AcceptRejectDelay { get; set; }
        public bool IsMarketTrader { get; set; }

        public Customer(BuySell buySell, decimal? amount, decimal? value)
        {
            BuySell = buySell;
            Amount = amount;
            Value = value;
        }

        public List<ISimEvent> CreateEvents(SimulationDependencyFactory dependencyFactory, DateTime simStart)
        {
            var events = new List<ISimEvent>();

            if (IsMarketTrader)
            {
                events.Add(new MarketTradeEvent(dependencyFactory, simStart.AddSeconds(RequestDelay), Amount, Value, BuySell));
                return events;
            }

            var priceRequest = new NewPriceRequestEvent(dependencyFactory.PricingEngine, simStart.AddSeconds(RequestDelay), Amount, Value, BuySell);
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
