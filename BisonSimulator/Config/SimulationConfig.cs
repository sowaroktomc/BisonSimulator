using Sowalabs.Bison.Common.Environment;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.Config
{
    /// <summary>
    /// Simulation configuration description.
    /// </summary>
    internal class SimulationConfig {
        /// <summary>
        /// Gests or sets simulation start date and time.
        /// </summary>
        public DateTime? FromDateTime { get; set; }
        /// <summary>
        /// Gests or sets simulation end date and time.
        /// </summary>
        public DateTime? ToDateTime { get; set; }
        /// <summary>
        /// Gets or sets crypto currency to simulate orders / customers for.
        /// </summary>
        public Crypto? Crypto { get; set; }

        /// <summary>
        /// Gets or sets a list of scenarios to simulate.
        /// </summary>
        public List<SimulationScenario> Scenarios { get; set; }
    }

}
