using Sowalabs.Bison.ProfitSim.Dependencies;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class BlockSimulationEvent : ISimEvent
    {
        private readonly SimulatedResetEvent _resetEvent;
        public BlockSimulationEvent(SimulatedResetEvent resetEvent, SimulationEngine engine) 
        {
            SimTime = engine.CurrentTime;
            _resetEvent = resetEvent;
        }

        public DateTime SimTime { get; }
        public void Simulate()
        {
            _resetEvent.Block();
        }
    }
}
