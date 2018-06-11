using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.Pricing.Dependencies
{
    public interface IMarketApi
    {
        OrderBook GetOrderBook();
    }
}
