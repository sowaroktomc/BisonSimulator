using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulatedTimerFactory : ITimerFactory
    {
        private readonly SimulationEngine _engine;

        public SimulatedTimerFactory(SimulationEngine engine)
        {
            _engine = engine;
        }

        public ITimer CreateTimer()
        {
            return new SimTimer(_engine);
        }
    }
}
