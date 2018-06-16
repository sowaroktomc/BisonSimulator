using Sowalabs.Bison.Common.Extensions;
using System;

namespace Sowalabs.Bison.Pricing.Strategies
{
    public class SimpleSpreadStrategy : IPricingStrategy
    {
        private readonly PricingEngine _engine;

        public decimal BuySpread { get; set; }
        public decimal SellSpread { get; set; }

        public SimpleSpreadStrategy(PricingEngine engine)
        {
            _engine = engine;
        }

        public decimal GetBuyPrice(decimal? requestedAmount, decimal? requestedValue)
        {
            decimal value;
            decimal amount;

            if (!requestedAmount.HasValue && !requestedValue.HasValue)
            {
                throw new Exception("Requested amount or value should've been given.");
            }
            if (requestedAmount.HasValue)
            {
                value = _engine.OrderBook.Asks.GetTopEntriesValue(requestedAmount.Value);
                amount = requestedAmount.Value;
            }
            else
            {
                value = requestedValue.Value;
                amount = _engine.OrderBook.Asks.GetTopEntriesAmount(requestedValue.Value);
            }

            var valueWithSpread = value * (1 + BuySpread);

            // with small volumes we want at least 1 cent margin!
            if (Math.Round(value, 2) - Math.Round(valueWithSpread, 2) == 0)
            {
                if (requestedAmount.HasValue)
                {
                    return (value + 0.01m) / amount;
                }

                return (value - 0.01m) / amount;
            }

            return valueWithSpread / amount;
        }

        public decimal GetSellPrice(decimal? requestedAmount, decimal? requestedValue)
        {
            decimal value;
            decimal amount;

            if (!requestedAmount.HasValue && !requestedValue.HasValue)
            {
                throw new Exception("Requested amount or value should've been given.");
            }
            if (requestedAmount.HasValue)
            {
                value = _engine.OrderBook.Bids.GetTopEntriesValue(requestedAmount.Value);
                amount = requestedAmount.Value;
            }
            else
            {
                value = requestedValue.Value;
                amount = _engine.OrderBook.Bids.GetTopEntriesAmount(requestedValue.Value);
            }

            var valueWithSpread = value * (1 - SellSpread);

            // with small volumes we want at least 1 cent margin!
            if (Math.Round(value, 2) - Math.Round(valueWithSpread, 2) == 0)
            {
                if (requestedAmount.HasValue)
                {
                    return (value - 0.01m) / amount;
                }

                return (value + 0.01m) / amount;
            }

            return valueWithSpread / amount;
        }
    }
}
