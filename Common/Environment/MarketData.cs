using System.Collections.Generic;

namespace Sowalabs.Bison.Common.Environment
{
    internal class MarketData
    {
        public BankData Bank { get; }

        public Market Market { get; }

        public MarketData(Market market)
        {
            this.Market = market;
            this.Bank = new BankData(market + "BankSwiftCode");

            // TODO Load from settings.
            this.Bank.AddAccount(new AccountData { Currency = Currency.Eur, Iban = "Eur@" + market});
            this.Bank.AddAccount(new AccountData { Currency = Currency.Usd, Iban = "Usd@" + market });
        }

        private static readonly Dictionary<Market, MarketData> Cache = new Dictionary<Market, MarketData>();

        static MarketData()
        {
            //TODO Load from settings.
            Cache.Add(Market.Bitstamp, new MarketData(Market.Bitstamp));
        }

        public static MarketData GetMarketData(Market market)
        {
            return Cache[market];
        }

    }
}
