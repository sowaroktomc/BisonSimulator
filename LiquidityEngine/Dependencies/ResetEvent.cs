using System;
using System.Threading;

namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    internal class ResetEvent : IResetEvent
    {
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public void Set()
        {
            _resetEvent.Set();
        }

        public bool Wait(int millisecondsTimeout)
        {
            return _resetEvent.WaitOne(millisecondsTimeout);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _resetEvent.Dispose();
        }
    }
}
