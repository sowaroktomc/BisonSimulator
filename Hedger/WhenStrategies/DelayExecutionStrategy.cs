using System;
using Sowalabs.Bison.Common.Timer;
using Sowalabs.Bison.Hedger.Dependencies;

namespace Sowalabs.Bison.Hedger.WhenStrategies
{
    public class DelayExecutionStrategy : IWhenStrategy
    {
        private bool _hasOffers;
        private readonly ITimer _timer;
        private readonly HedgingEngine _engine;

        public double Delay
        {
            get => this._timer.Interval;
            set => this._timer.Interval = value;
        }

        public DelayExecutionStrategy(HedgingEngine engine, IDependencyFactory dependencyFactory)
        {
            this._engine = engine;
            this._timer = dependencyFactory.TimerFactory.CreateTimer();
            this._timer.Interval = this.Delay;
            this._timer.Action = Execute;
        }

        public void RegisterAcceptedOffer(Common.Trading.Offer offer)
        {
            if (this._hasOffers)
            {
                return;
            }

            this._hasOffers = true;
            this._timer.Start();
        }

        private void Execute()
        {
            this._hasOffers = false;
            this._engine.ExecuteOffers();
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._timer.Dispose();
            }
        }

        #endregion

    }
}
