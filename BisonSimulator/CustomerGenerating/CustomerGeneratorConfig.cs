using Sowalabs.Bison.Common.Trading;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.CustomerGenerating
{
    /// <summary>
    /// Configuration for generating simulated customers.
    /// </summary>
    internal class CustomerGeneratorConfig
    {
        /// <summary>
        /// Number of simulated customers.
        /// </summary>
        public int NumCustomers { get; set; }

        /// <summary>
        /// List of sizes (values) of simulated customer's orders.
        /// </summary>
        public List<decimal> OrderSizes { get; }

        /// <summary>
        /// Number of seconds the customer has to accept or reject a price offer.
        /// </summary>
        public int ReservationPeriod { get; set; }

        /// <summary>
        /// Rate of simulated offer acceptance (in %).
        /// </summary>
        public double OfferAcceptanceRate { get; set; }

        /// <summary>
        /// Length of simulated time slot (in seconds).
        /// </summary>
        public int SimulatedTime { get; set; }

        /// <summary>
        /// Whether generated customers are buying or selling.
        /// </summary>
        public BuySell CreateBuysOrSells { get; set; }

        /// <summary>
        /// Configuration for generating simulated customers.
        /// </summary>
        public CustomerGeneratorConfig()
        {
            OrderSizes = new List<decimal>();


            // Set default values.
            ReservationPeriod = 10;
            OfferAcceptanceRate = 100;
            SimulatedTime = 600;
            NumCustomers = 1;
        }
    }
}
