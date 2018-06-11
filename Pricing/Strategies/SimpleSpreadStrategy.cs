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
            return this._engine.OrderBook.Asks.First().Price * (1 + this.BuySpread);
        }

        public decimal GetSellPrice(decimal volume)
        {
            return this._engine.OrderBook.Bids.First().Price * (1 - this.SellSpread);
        }
    }
}
