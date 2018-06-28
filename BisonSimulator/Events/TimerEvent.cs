using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Simulates a timer event.
    /// </summary>
    internal class TimerEvent : ISimEvent
    {

        private readonly Action _action;

        public DateTime SimTime { get; set; }

        /// <summary>
        /// Simulates a timer event.
        /// </summary>
        /// <param name="actionTime">Date and time the timer fires.</param>
        /// <param name="action">Action executed when timer fires.</param>
        public TimerEvent(DateTime actionTime, Action action)
        {
            SimTime = actionTime;
            _action = action;
        }

        public void Simulate()
        {
            _action();
        }
    }
}
