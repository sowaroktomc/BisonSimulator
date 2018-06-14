using System;
using System.Linq;

namespace Sowalabs.Bison.Pricing.Strategies
{
    public class SimpleSpreadStrategy : IPricingStrategy
    {
        private readonly PricingEngine _engine;

        public decimal BuySpread { get; set; }
        public decimal SellSpread { get; set; }

        public SimpleSpreadStrategy(PricingEngine engine)
        {
            this._engine = engine;
        }

        public decimal GetBuyPrice(decimal volume)
        {
            var price = this._engine.OrderBook.Asks.First().Price;
            var value = volume * price;
            var valueWithSpread = value * (1 + this.BuySpread);

            // with small volumes we want at least 1 cent margin!
            if (Math.Round(value, 2) - Math.Round(valueWithSpread, 2) == 0)
            {
                return (value + 0.01m) / volume;
            }

            return valueWithSpread / volume;
        }

        public decimal GetSellPrice(decimal volume)
        {
            var price = this._engine.OrderBook.Asks.First().Price;
            var value = volume * price;
            var valueWithSpread = value * (1 - this.SellSpread);

            // with small volumes we want at least 1 cent margin!
            if (Math.Round(value, 2) - Math.Round(valueWithSpread, 2) == 0)
            {
                return (value - 0.01m) / volume;
            }

            return valueWithSpread / volume;
        }
    }
}
