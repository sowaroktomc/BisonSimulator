using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.Common.Trading
{
    public class OrderBook
    {
        public DateTime AcqTime { get; set; }

        public List<OrderBookEntry> Bids { get; set; }
        public List<OrderBookEntry> Asks { get; set; }

        public OrderBook()
        {
            this.Bids = new List<OrderBookEntry>();
            this.Asks = new List<OrderBookEntry>();
        }
    }
}
