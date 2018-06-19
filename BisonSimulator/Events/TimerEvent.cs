using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class TimerEvent : ISimEvent
    {

        private readonly Action _action;
        private DateTime _simTime;

        public DateTime SimTime { get { return _simTime; } set { _simTime = value; } }

        public TimerEvent(DateTime actionTime, Action action)
        {
            this._simTime = actionTime;
            this._action = action;
        }

        public void Simulate()
        {
            this._action();
        }
    }
}
