using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.Entity
{
    /// <summary>
    /// Profit simulation result data.
    /// </summary>
    internal class ProfitSimulationResult
    {

        /// <summary>
        /// A result of a single profit simulation run.
        /// </summary>
        public class PlEntry
        {
            public PlEntry(DateTime time, decimal pl)
            {
                DateTime = time;
                PL = pl;
            }

            /// <summary>
            /// Date and time when simulate time starts.
            /// </summary>
            public DateTime DateTime { get; set; }

            /// <summary>
            /// Profit / loss.
            /// </summary>
            public decimal PL { get; set; }

            /// <summary>
            /// Market price of crypto-currency at simulation start time.
            /// </summary>
            public decimal Price { get; set; }
        }
        
        /// <summary>
        /// List of result entries - one for each simulation run.
        /// </summary>
        public List<PlEntry> Entries { get; }

        /// <summary>
        /// Profit simulation result data.
        /// </summary>
        public ProfitSimulationResult()
        {
            this.Entries = new List<PlEntry>();
        }
    }
}