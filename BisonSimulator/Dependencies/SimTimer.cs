using System;
using Sowalabs.Bison.Common.Timer;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    /// <summary>
    /// Simlated timer class - enables execution after a specified amount of time elapses (simulated time).
    /// </summary>
    internal class SimTimer : ITimer
    {
        private TimerEvent _event;
        private readonly SimulationEngine _engine;

        /// <summary>
        /// Gets or sets number of milliseconds after which timer elapses.
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// Gets or sets action to be executed when timer elapses.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Simulated timer - executes an event at given simulated time.
        /// </summary>
        /// <param name="engine">Engine to use for time simulation.</param>
        public SimTimer(SimulationEngine engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Starts timer -> adds a timer event into simulation engine.
        /// </summary>
        public void Start()
        {
            _event = new TimerEvent(_engine.CurrentTime.AddMilliseconds(Interval), FireTimer);
            _engine.AddEvent(_event);
        }

        /// <summary>
        /// Executes timer action.
        /// </summary>
        private void FireTimer()
        {
            Action();
            _event = null;
        }

        /// <summary>
        /// Stops timer -> removes timer event from simulation engine.
        /// </summary>
        public void Stop()
        {
            if (_event == null)
            {
                return;
            }

            _engine.CancelEvent(_event);
            _event = null;
        }

        /// <summary>
        /// Disposes of timer.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
