using Sowalabs.Bison.Common.Extensions;
using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulatedMarketApi : Pricing.Dependencies.IMarketApi, Hedger.Dependencies.IMarketApi
    {

        public OrderBook CurrentOrderBook { get; set; }

        public decimal MoneyBalance { get; set; }
        public decimal CryptoBalance { get; set; }

        public OrderBook GetOrderBook()
        {
            return this.CurrentOrderBook;
        }

        public void ExecuteSell(decimal amount, decimal limitPrice)
        {
            this.MoneyBalance += this.CurrentOrderBook.Bids.GetTopEntriesValue(amount);
            this.CryptoBalance -= amount;
        }

        public void ExecuteBuy(decimal amount, decimal limitPrice)
        {
            this.MoneyBalance -= this.CurrentOrderBook.Asks.GetTopEntriesValue(amount);
            this.CryptoBalance += amount;
        }

        public void Reset()
        {
            this.MoneyBalance = 0;
            this.CryptoBalance = 0;
        }
    }
}
