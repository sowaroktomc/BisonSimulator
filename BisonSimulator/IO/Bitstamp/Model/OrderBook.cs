using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model
{
    public class OrderBook
    {
        public DateTime AcqTime { get; set; }

        public List<Offer> Bids { get; set; }
        public List<Offer> Asks { get; set; }
    }
}
