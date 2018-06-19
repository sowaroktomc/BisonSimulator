using System;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class OrderBookEvent : ISimEvent
    {
        private readonly OrderBook _orderBook;
        private readonly SimulatedMarketApi _marketApi;
        private DateTime _simTime;

        public DateTime SimTime { get { return _simTime; } set { _simTime = value; } }

        public OrderBookEvent(OrderBook orderBook, SimulatedMarketApi marketApi)
        {
            this._orderBook = orderBook;
            this._marketApi = marketApi;
            _simTime = orderBook.AcqTime;
        }

        public void Simulate()
        {
            this._marketApi.CurrentOrderBook = this._orderBook;
        }
    }
}
