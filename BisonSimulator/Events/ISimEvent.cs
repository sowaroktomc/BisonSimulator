using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <summary>
    /// Interface for all simulated events.
    /// </summary>
    internal interface ISimEvent
    {

        /// <summary>
        /// Date and time at which the event takes place.
        /// </summary>
        DateTime SimTime { get; }

        /// <summary>
        /// Simulates the event.
        /// </summary>
        void Simulate();
    }
}
