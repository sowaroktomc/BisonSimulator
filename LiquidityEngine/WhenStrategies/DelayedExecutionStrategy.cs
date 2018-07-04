using System;
using System.Threading;
using Sowalabs.Bison.LiquidityEngine.Dependencies;

namespace Sowalabs.Bison.LiquidityEngine.WhenStrategies
{
    public class DelayedExecutionStrategy : IWhenStrategy
    {
        public interface IDelayedExecutionStrategyConsumer : IWhenStrategyConsumer
        {
            int Delay { get; }
        }

        private readonly object _locker = new object();
        private readonly IDependencyFactory _dependencyFactory;
        private readonly IResetEvent _resetEvent;
        private readonly Thread _thread;
        private readonly IDelayedExecutionStrategyConsumer _consumer;
        private bool _isRunning;
        private DateTime? _firstNewOffer;


        public DelayedExecutionStrategy(IDelayedExecutionStrategyConsumer consumer, IDependencyFactory dependencyFactory)
        {
            _consumer = consumer;
            _dependencyFactory = dependencyFactory;
            _consumer.OnExecute += OnConsumerExecute;

            _resetEvent = _dependencyFactory.CreateResetEvent();
            _thread = new Thread(ExecuteContinously);
            _isRunning = true;
            _thread.Start();
        }

        private void OnConsumerExecute(object sender, EventArgs e)
        {
            lock (_locker)
            {
                _firstNewOffer = null;
            }
        }

        private void ExecuteContinously()
        {
            while (_isRunning)
            {
                bool isExecuteNeeded;
                lock (_locker)
                {
                    isExecuteNeeded = _firstNewOffer.HasValue && _dependencyFactory.DateTimeProvider.Now >= _firstNewOffer.Value.AddSeconds(_consumer.Delay);
                }
                if (isExecuteNeeded)
                {
                    _consumer.Execute();
                }

                _resetEvent.Wait(1000);
            }
        }

        public void RegisterNewEntry()
        {
            lock (_locker)
            {
                if (_firstNewOffer.HasValue)
                {
                    return;
                }

                _firstNewOffer = _dependencyFactory.DateTimeProvider.Now;
                _resetEvent.Set(); // Start checking immediately 
            }
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
                _isRunning = false;
                _resetEvent.Set();
                _thread.Join();
                _resetEvent.Dispose();
            }
        }

        #endregion

    }
}
