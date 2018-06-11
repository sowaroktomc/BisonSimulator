using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class TimerEvent : ISimEvent
    {

        private readonly Action _action;

        public TimerEvent(DateTime actionTime, Action action)
        {
            this.SimTime = actionTime;
            this._action = action;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public DateTime SimTime { get; }
        public void Simulate()
        {
            this._action();
        }
    }
}
