using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    /// <summary>
    /// A simulated timer factory. Creates simulated timer instances.
    /// </summary>
    internal class SimulatedTimerFactory : ITimerFactory
    {
        private readonly SimulationEngine _engine;

        /// <summary>
        /// A simulated timer factory. Creates simulated timer instances.
        /// </summary>
        /// <param name="engine">Simulation engine used to simulate time.</param>
        public SimulatedTimerFactory(SimulationEngine engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Creates a timer.
        /// </summary>
        /// <returns>New timer.</returns>
        public ITimer CreateTimer()
        {
            return new SimTimer(_engine);
        }
    }
}
