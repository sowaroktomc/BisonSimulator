using System;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model
{
    /// <summary>
    /// Order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time of order.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Ordered amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Ordered price.
        /// </summary>
        public decimal Price { get; set; }
    }
}
