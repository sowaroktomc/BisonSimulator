using Sowalabs.Bison.LiquidityEngine;
using Sowalabs.Bison.LiquidityEngine.Tasks;

namespace Sowalabs.Bison.ProfitSim.SimulationMoqs
{
    /// <summary>
    /// A moq class for overriding LiquidityEngine Queue singleton instance.
    /// </summary>
    public class LiquidityEngineQueueMoq : Queue
    {
        private static LiquidityEngineQueueMoq _instance;
        private static readonly object Padlock = new object();

        /// <summary>
        /// REturns a singleton instance of LiquidityEngineQueueMoq class.
        /// </summary>
        /// <returns>A singleton instance of LiquidityEngineQueueMoq class.</returns>
        public static LiquidityEngineQueueMoq GetSingleton()
        {
            lock (Padlock)
            {
                return _instance ?? (_instance = new LiquidityEngineQueueMoq());
            }
        }

        /// <summary>
        /// Ensures a singleton instance is created and overrides LiquidityEngine Queue singleton instance.
        /// </summary>
        public static void EnsureInstanceCreated()
        {
            GetSingleton();
        }

        /// <summary>
        /// A moq class for overriding LiquidityEngine Queue singleton instance.
        /// </summary>
        public LiquidityEngineQueueMoq()
        {
            SingletonInstance = this;
        }

        /// <summary>
        /// Enqueues given Liquidity engine task - executes it immediately.
        /// </summary>
        /// <param name="task">Task to be enqueued into queue.</param>
        public override void Enqueue(BaseTask task)
        {
            // Immediate execution.
            task.Execute();
        }
    }
}
