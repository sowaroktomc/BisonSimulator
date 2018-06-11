using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.Hedger.Dependencies
{
    internal class BitstampMarketApi : IMarketApi
    {
        public OrderBook GetOrderBook()
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteSell(decimal amount, decimal limitPrice)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteBuy(decimal amount, decimal limitPrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
