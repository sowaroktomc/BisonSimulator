using Sowalabs.Bison.LiquidityEngine;
using Sowalabs.Bison.LiquidityEngine.Tasks;
using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.SimulationMoqs
{
    internal class SimulationQueue : IQueue
    {

        private readonly SimulationDependencyFactory _dependencyFactory;

        public SimulationQueue(SimulationDependencyFactory dependencyFactory)
        {
            _dependencyFactory = dependencyFactory;
        }

        public void Dispose()
        {
            
        }

        public void Enqueue(BaseTask task)
        {
            if (task.ShouldExecute())
            {
                task.Execute();
            }
            else
            {
                var timer = _dependencyFactory.TimerFactory.CreateTimer();
                timer.Action = () =>
                {
                    Enqueue(task);
                    timer.Dispose();
                };
                timer.Interval = (task.ExecuteAtTime - _dependencyFactory.DateTimeProvider.Now).TotalMilliseconds;
                timer.Start();
            }
        }
    }
}
