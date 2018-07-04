using Sowalabs.Bison.ProfitSim.Dependencies;

namespace Sowalabs.Bison.ProfitSim.SimulationMoqs
{
    internal class SimulationLiquidityEngineMoq : LiquidityEngine.LiquidityEngine
    {
        
        public SimulationLiquidityEngineMoq(SimulationDependencyFactory dependencyFactory) : base(dependencyFactory, new SimulationQueue(dependencyFactory))
        {
        }
    }
}
