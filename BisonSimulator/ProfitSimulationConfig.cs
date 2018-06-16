using Sowalabs.Bison.ProfitSim.Model;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sowalabs.Bison.Common.Environment;

namespace Sowalabs.Bison.ProfitSim
{
    class ProfitSimulationConfig
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

        public List<Customer> Customers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Crypto Crypto { get; set; }

        public string OutputFilename { get; set; }

        public ProfitSimulationConfig()
        {
            Customers = new List<Customer>();

            HedgingDelay = 0;
            BuySpread = 0.2m;
            SellSpread = 0.2m;
        }

        public ProfitSimulationConfig Clone()
        {
            return new ProfitSimulationConfig
            {
                HedgingDelay = this.HedgingDelay,
                BuySpread = BuySpread,
                SellSpread = SellSpread,
                Crypto = Crypto,
                OutputFilename = OutputFilename,
                Customers = Customers.Select(customer => customer.Clone()).ToList()
            };
        }
    }
}
