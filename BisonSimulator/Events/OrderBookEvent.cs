using System;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OrderBookEvent : ISimEvent
    {
        private readonly OrderBook _orderBook;
        private readonly SimulatedMarketApi _marketApi;

        public OrderBookEvent(OrderBook orderBook, SimulatedMarketApi marketApi)
        {
            this._orderBook = orderBook;
            this._marketApi = marketApi;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public DateTime SimTime => this._orderBook.AcqTime;

        public void Simulate()
        {
            this._marketApi.CurrentOrderBook = this._orderBook;
        }
    }
}
