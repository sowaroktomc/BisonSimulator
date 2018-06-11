using System;
using Sowalabs.Bison.Common.Timer;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimTimer : ITimer
    {
        private TimerEvent _event;

        public double Interval { get; set; }
        public Action Action { get; set; }
        public void Start()
        {
            this._event = new TimerEvent(SimulationEngine.Instance.CurrentTime.AddMilliseconds(this.Interval), this.Action);
            SimulationEngine.Instance.AddEvent(this._event);
        }

        public void Stop()
        {
            SimulationEngine.Instance.CancelEvent(this._event);
            this._event = null;
        }

        public void Dispose()
        {
            if (this._event != null)
            {
                this.Stop();
            }
        }
    }
}
