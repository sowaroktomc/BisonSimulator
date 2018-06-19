using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    internal class HistoryEnumerator : IHistoryEnumerator
    {
        private int _currentIndex;
        private readonly BitstampHistoryLoader _loader;
        private System.Threading.Mutex _locker = new System.Threading.Mutex();
        private bool _isNewHistoryJustLoaded;


        private readonly List<OrderBook> _history = new List<OrderBook>();

        public HistoryEnumerator(BitstampHistoryLoader loader)
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
                _isNewHistoryJustLoaded = true;
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
                if (_currentIndex >= _history.Count)
                {
                    if (!LoadNewData())
                    {
                        return null;
                    }
                }

                return _history[_currentIndex++];
            } finally
            {
                _locker.ReleaseMutex();
            }
        }


        public bool Restart()
        {
            _locker.WaitOne();
            try
            { _history.RemoveAt(0);
                _currentIndex = 0;

                if (_history.Count == 0)
                {
                    return LoadNewData();
                }

                return true;
            } finally
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
                } finally
                {
                    _locker.WaitOne();
                }
            }
        }
    }
}
