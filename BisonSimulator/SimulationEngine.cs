using Sowalabs.Bison.ProfitSim.Events;
using System;
using Sowalabs.Bison.Common;

namespace Sowalabs.Bison.ProfitSim
{
    internal class SimulationEngine
    {
        
        private readonly SimEventQueue _queue = new SimEventQueue();

        public DateTime CurrentTime { get; private set; }

        public event EventHandler<EventArgs<ISimEvent>> AfterEventSimulation;

        private void FireAfterEventSimulation(ISimEvent simEvent)
        {
            this.AfterEventSimulation?.Invoke(this, new EventArgs<ISimEvent>(simEvent));
        }

        public void AddEvent(ISimEvent simEvent)
        {
            this._queue.Enqueue(simEvent);
        }
        public void CancelEvent(ISimEvent simEvent)
        {
            this._queue.Remove(simEvent);
        }

        public ISimEvent PeekLastEvent()
        {
            return this._queue.PeekLast();
        }

        public void Execute()
        {
            ISimEvent simEvent;
            while ((simEvent = this._queue.Dequeue()) != null)
            {
                this.CurrentTime = simEvent.SimTime;
                simEvent.Simulate();
                this.FireAfterEventSimulation(simEvent);
            }
        }
    }
}
