using System;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Simulates a new orderbook on market.
    /// </summary>
    internal class OrderBookEvent : ISimEvent
    {
        private readonly OrderBook _orderBook;
        private readonly SimulatedMarketApi _marketApi;

        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulates a new orderbook on market.
        /// </summary>
        /// <param name="orderBook">New orderbook.</param>
        /// <param name="marketApi">Simulated market on which new orderbook comes to existence.</param>
        public OrderBookEvent(OrderBook orderBook, SimulatedMarketApi marketApi)
        {
            _orderBook = orderBook;
            _marketApi = marketApi;
            SimTime = orderBook.AcqTime;
        }

        public void Simulate()
        {
            _marketApi.CurrentOrderBook = _orderBook;
        }
    }
}
