using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using Sowalabs.Bison.Common.Extensions;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <summary>
    /// Simulates a trade being executed on market.
    /// </summary>
    class MarketTradeEvent : ISimEvent
    {
        private readonly SimulationDependencyFactory _dependencyFactory;
        private readonly decimal? _amount;
        private readonly decimal? _value;
        private readonly BuySell _buySell;

        /// <inheritdoc />
        /// <summary>
        /// Date and time at which the event takes place.
        /// </summary>
        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulates a trade being executed on market.
        /// </summary>
        /// <param name="dependencyFactory"></param>
        /// <param name="requestAtTime">Date and time at which the event takes place.</param>
        /// <param name="requestAmount">Amount to be traded. If not given then requestValue is required.</param>
        /// <param name="requestValue">Value to be traded. If not given then requestAmount is required</param>
        /// <param name="buySell">Is crypto-currency bought or sold.</param>
        public MarketTradeEvent(SimulationDependencyFactory dependencyFactory, DateTime requestAtTime, decimal? requestAmount, decimal? requestValue, BuySell buySell)
        {
            _dependencyFactory = dependencyFactory;
            _amount = requestAmount;
            _value = requestValue;
            _buySell = buySell;

            SimTime = requestAtTime;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates the event.
        /// </summary>
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
