using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    internal class HistoryQueue : IHistoryEnumerator
    {
        private readonly BitstampHistoryLoader _loader;
        private readonly List<OrderBook> _history = new List<OrderBook>();
        private System.Threading.Mutex _locker = new System.Threading.Mutex();
        private bool _isNewHistoryJustLoaded;

        public HistoryQueue(BitstampHistoryLoader loader)
        {
            _loader = loader;
            _loader.RegisterEnumerator(this);
        }

        public void AppendHistory(List<OrderBook> history)
        {
            _locker.WaitOne();
            try
            {
                _history.AddRange(history);
            }
            finally
            {
                _locker.ReleaseMutex();
            }
        }

        public OrderBook GetNext()
        {
            _locker.WaitOne();
            try
            {
                if (_history.Count == 0)
                {
                    if (!LoadNewData())
                    {
                        return null;
                    }
                }

                var next = _history[0];
                _history.RemoveAt(0);
                return next;
            }
            finally
            {
                _locker.ReleaseMutex();
            }
        }

        private bool LoadNewData()
        {
            _isNewHistoryJustLoaded = false;
            _locker.ReleaseMutex();
            lock (_loader.SynchronizationContext)
            {
                try
                {
                    if (_isNewHistoryJustLoaded)
                    {
                        return true;
                    }
                    return _loader.LoadData();
                }
                finally
                {
                    _locker.WaitOne();
                }
            }
        }
    }
}
