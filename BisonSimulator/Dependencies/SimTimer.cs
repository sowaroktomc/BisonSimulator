using System;
using Sowalabs.Bison.Common.Timer;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimTimer : ITimer
    {
        private TimerEvent _event;
        private readonly SimulationEngine _engine;

        public double Interval { get; set; }
        public Action Action { get; set; }

        public SimTimer(SimulationEngine engine)
        {
            _engine = engine;
        } 

        public void Start()
        {
            this._event = new TimerEvent(_engine.CurrentTime.AddMilliseconds(this.Interval), this.Action);
            _engine.AddEvent(this._event);
        }

        public void Stop()
        {
            _engine.CancelEvent(this._event);
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
