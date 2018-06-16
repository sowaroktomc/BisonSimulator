using Sowalabs.Bison.Common.Extensions;
using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulatedMarketApi : Pricing.Dependencies.IMarketApi, Hedger.Dependencies.IMarketApi
    {
        public OrderBook CurrentOrderBook { get; set; }
        public bool SubstractAmountsFromBooks { get; set; }

        public decimal MoneyBalance { get; set; }

        public decimal CryptoBalance => BoughtAmount - SoldAmount;

        public decimal BoughtAmount { get; set; }
        public decimal SoldAmount { get; set; }

        public OrderBook GetOrderBook()
        {
            return CurrentOrderBook;
        }

        public void ExecuteSell(decimal amount, decimal limitPrice)
        {
            var adjustAmount = 0m;
            if (SubstractAmountsFromBooks)
            {
                adjustAmount = CryptoBalance;
            }
            var totalValue = CurrentOrderBook.Bids.GetTopEntriesValue(adjustAmount + amount);
            var value = totalValue - CurrentOrderBook.Bids.GetTopEntriesValue(adjustAmount);


            MoneyBalance += value;
            SoldAmount += amount;
        }

        public void ExecuteBuy(decimal amount, decimal limitPrice)
        {

            var adjustAmount = 0m;
            if (SubstractAmountsFromBooks)
            {
                adjustAmount = CryptoBalance;
            }
            var totalValue = CurrentOrderBook.Asks.GetTopEntriesValue(adjustAmount + amount);
            var value = totalValue - CurrentOrderBook.Asks.GetTopEntriesValue(adjustAmount);


            MoneyBalance -= value;
            BoughtAmount += amount;
        }

        public void Reset()
        {
            MoneyBalance = 0;
            SoldAmount = 0;
            BoughtAmount = 0;
        }
    }
}
