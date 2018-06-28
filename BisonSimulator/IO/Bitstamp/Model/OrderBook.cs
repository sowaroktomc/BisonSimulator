using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model
{
    /// <summary>
    /// Order book entry.
    /// </summary>
    public class OrderBook
    {
        /// <summary>
        /// Time of acquisition.
        /// </summary>
        public DateTime AcqTime { get; set; }

        /// <summary>
        /// Bids.
        /// </summary>
        public List<Offer> Bids { get; set; }

        /// <summary>
        /// Asks.
        /// </summary>
        public List<Offer> Asks { get; set; }
    }
}
