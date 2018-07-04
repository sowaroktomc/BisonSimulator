using Sowalabs.Bison.ProfitSim.Dependencies;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    class SetSimulatedResetEventEvent : ISimEvent
    {
        private readonly SimulatedResetEvent _resetEvent;

        public SetSimulatedResetEventEvent(SimulatedResetEvent resetEvent, DateTime simulateAtTime)
        {
            SimTime = simulateAtTime;
            _resetEvent = resetEvent;
        }

        public DateTime SimTime { get; }
        public void Simulate()
        {
            _resetEvent.SetEvent();
        }
    }
}
