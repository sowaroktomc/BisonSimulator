using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using Sowalabs.Bison.Common.Extensions;

namespace Sowalabs.Bison.ProfitSim.Events
{
    class MarketTradeEvent : ISimEvent
    {
        private readonly SimulationDependencyFactory _dependencyFactory;
        private readonly decimal? _amount;
        private readonly decimal? _value;
        private readonly BuySell _buySell;

        public Guid Id { get; }
        public DateTime SimTime { get; }

        public MarketTradeEvent(SimulationDependencyFactory dependencyFactory, DateTime requestAtTime, decimal? requestAmount, decimal? requestValue, BuySell buySell)
        {
            _dependencyFactory = dependencyFactory;
            _amount = requestAmount;
            _value = requestValue;
            _buySell = buySell;

            SimTime = requestAtTime;

            Id = Guid.NewGuid();
        }

        public void Simulate()
        {
            switch (_buySell)
            {
                case BuySell.Buy:
                    if (_amount.HasValue)
                    {
                        _dependencyFactory.BitcoinMarketApi.BoughtAmount += _amount.Value;
                        return;
                    }

                    if (!_value.HasValue)
                    {
                        return;
                    }

                    var boughtValue = _dependencyFactory.BitcoinMarketApi.CurrentOrderBook.Asks.GetTopEntriesValue(_dependencyFactory.BitcoinMarketApi.BoughtAmount);
                    _dependencyFactory.BitcoinMarketApi.BoughtAmount = _dependencyFactory.BitcoinMarketApi.CurrentOrderBook.Asks.GetTopEntriesAmount(boughtValue + _value.Value);

                    return;
                case BuySell.Sell:
                    if (_amount.HasValue)
                    {
                        _dependencyFactory.BitcoinMarketApi.SoldAmount += _amount.Value;
                        return;
                    }
                    if (!_value.HasValue)
                    {
                        return;
                    }

                    var soldValue = _dependencyFactory.BitcoinMarketApi.CurrentOrderBook.Bids.GetTopEntriesValue(_dependencyFactory.BitcoinMarketApi.SoldAmount);
                    _dependencyFactory.BitcoinMarketApi.SoldAmount = _dependencyFactory.BitcoinMarketApi.CurrentOrderBook.Bids.GetTopEntriesAmount(soldValue + _value.Value);

                    return;
            }
        }
    }
}
