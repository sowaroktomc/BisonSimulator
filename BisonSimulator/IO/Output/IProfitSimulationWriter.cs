using System;
using Sowalabs.Bison.ProfitSim.Entity;

namespace Sowalabs.Bison.ProfitSim.IO.Output
{
    /// <summary>
    /// Defines a writer for outpouting profit simulation results. 
    /// </summary>
    internal interface IProfitSimulationWriter : IDisposable
    {
        /// <summary>
        /// Writes given entry to output.
        /// </summary>
        /// <param name="entry">Entry to write.</param>
        void Write(ProfitSimulationResult.PlEntry entry);

        /// <summary>
        /// Initializes writer.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Closes writer.
        /// </summary>
        void Close();
    }
}
