using Sowalabs.Bison.ProfitSim.Events;
using System;
using Sowalabs.Bison.Common;

namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// Simulation engine responsbile for stacking simulation events into time queue and executing them in order.
    /// </summary>
    internal class SimulationEngine
    {
        private readonly SimEventQueue _queue = new SimEventQueue();

        /// <summary>
        /// Current time in simulation world.
        /// </summary>
        public DateTime CurrentTime { get; private set; }

        /// <summary>
        /// Event is fired each time after an event is simulated. 
        /// </summary>
        public event EventHandler<EventArgs<ISimEvent>> AfterEventSimulation;

        /// <summary>
        /// Fires an AfterEventSimulation event.
        /// </summary>
        /// <param name="simEvent">Simulation event that was just simulated.</param>
        private void FireAfterEventSimulation(ISimEvent simEvent)
        {
            AfterEventSimulation?.Invoke(this, new EventArgs<ISimEvent>(simEvent));
        }

        /// <summary>
        /// Adds a new event into simulation queue.
        /// </summary>
        /// <param name="simEvent">An event to be simulated.</param>
        public void AddEvent(ISimEvent simEvent)
        {
            _queue.Enqueue(simEvent);
        }

        /// <summary>
        /// Removes given event from simulation.
        /// </summary>
        /// <param name="simEvent">Eventt to be removed from simulation.</param>
        public void CancelEvent(ISimEvent simEvent)
        {
            _queue.Remove(simEvent);
        }

        /// <summary>
        /// Returns last event that is scheduled for simulation.
        /// </summary>
        /// <returns>Last event that is scheduled for simulation.</returns>
        public ISimEvent PeekLastEvent()
        {
            return _queue.PeekLast();
        }

        /// <summary>
        /// Executes simulation - simulates all scheduled events.
        /// While executing new events can be scheduled into queue and will also get simulated.
        /// </summary>
        public void Simulate()
        {
            ISimEvent simEvent;
            while ((simEvent = _queue.Dequeue()) != null)
            {
                CurrentTime = simEvent.SimTime;
                simEvent.Simulate();
                FireAfterEventSimulation(simEvent);
            }
        }
    }
}
