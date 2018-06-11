namespace Sowalabs.Bison.Hedger.Dependencies
{
    public interface IMarketApi
    {
        Common.Trading.OrderBook GetOrderBook();
        void ExecuteSell(decimal amount, decimal limitPrice);
        void ExecuteBuy(decimal amount, decimal limitPrice);
    }
}
