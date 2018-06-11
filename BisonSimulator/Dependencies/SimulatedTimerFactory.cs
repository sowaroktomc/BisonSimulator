using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    public class SimulatedTimerFactory : ITimerFactory
    {
        public ITimer CreateTimer()
        {
            return new SimTimer();
        }
    }
}
