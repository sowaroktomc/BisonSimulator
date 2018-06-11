using System;
using System.Timers;

namespace Sowalabs.Bison.Common.Timer
{
    public class BasicTimer : ITimer
    {
        private System.Timers.Timer _timer;

        public double Interval
        {
            get => this._timer.Interval;
            set => this._timer.Interval = value;
        }

        public Action Action { get; set; }

        public BasicTimer()
        {
            this._timer = new System.Timers.Timer();
            this._timer.Elapsed += OnTimerElapsed;
        }

        public void Start()
        {
            this._timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Action();
        }

        public void Stop()
        {
            this._timer.Stop();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._timer?.Dispose();
                this._timer = null;
            }
        }
    }
}
