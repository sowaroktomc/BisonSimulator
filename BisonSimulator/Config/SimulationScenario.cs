using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim.Config
{
    /// <summary>
    /// Simulated scenario description.
    /// </summary>
    internal class SimulationScenario
    {
        /// <summary>
        /// Number of seconds between price offer acceptance and hedging execution on market.
        /// </summary>
        public int HedgingDelay { get; set; }

        /// <summary>
        /// Spread between market best ask and Bison offered price on buys (in %).
        /// </summary>
        public decimal BuySpread { get; set; }

        /// <summary>
        /// Spread between market best bid and Bison offered price on sells (in %).
        /// </summary>
        public decimal SellSpread { get; set; }

        /// <summary>
        /// List of simulated customers.
        /// </summary>
        public List<Customer> Customers { get; set; }

        /// <summary>
        /// Crypto currency to simulate.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Crypto Crypto { get; set; }

        /// <summary>
        /// File to output results to.
        /// </summary>
        public string OutputFilename { get; set; }

        /// <summary>
        /// Simulation start date and time.
        /// </summary>
        public DateTime FromDateTime { get; set; }

        /// <summary>
        /// Simulation end date and time.
        /// </summary>
        public DateTime ToDateTime { get; set; }

        /// <summary>
        /// Simulated scenario description.
        /// </summary>
        public SimulationScenario()
        {
            Customers = new List<Customer>();

            HedgingDelay = 0;
            BuySpread = 0.2m;
            SellSpread = 0.2m;
        }

        /// <summary>
        /// Clones a simulated scenario description.
        /// </summary>
        public SimulationScenario Clone()
        {
            return new SimulationScenario
            {
                HedgingDelay = this.HedgingDelay,
                BuySpread = BuySpread,
                SellSpread = SellSpread,
                Crypto = Crypto,
                OutputFilename = OutputFilename,
                FromDateTime = FromDateTime,
                ToDateTime = ToDateTime,
                Customers = Customers.Select(customer => customer.Clone()).ToList()
            };
        }
    }
}
