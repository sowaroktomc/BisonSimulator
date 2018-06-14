using Sowalabs.Bison.LiquidityEngine;
using Sowalabs.Bison.LiquidityEngine.Tasks;

namespace Sowalabs.Bison.ProfitSim
{
    public class SimulationLiquidityEngineQueue : Queue
    {
        private static SimulationLiquidityEngineQueue _instance;
        private static readonly object _padlock = new object();


        public static SimulationLiquidityEngineQueue GetInstance()
        {
            lock (_padlock)
            {
                return _instance ?? (_instance = new SimulationLiquidityEngineQueue());
            }
        }

        public static void EnsureInstanceCreated()
        {
            GetInstance();
        }

        public SimulationLiquidityEngineQueue()
        {
            Queue.SingletonInstance = this;
        }

        public override void Enqueue(BaseTask task)
        {
            task.Execute();
        }
    }
}
