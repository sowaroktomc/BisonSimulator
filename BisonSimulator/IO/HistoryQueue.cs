using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    /// <summary>
    /// Iterates over order book history series and loads new data when needed.
    /// </summary>
    internal class HistoryQueue : IHistoryProvider
    {
        private readonly BitstampHistoryLoader _loader;
        private readonly LinkedList<OrderBook> _history = new LinkedList<OrderBook>();
        private readonly System.Threading.Mutex _locker = new System.Threading.Mutex();
        private int? _lastPeriodSynchronizationToken;

        public HistoryQueue(BitstampHistoryLoader loader)
        {
            _loader = loader;
            _loader.RegisterHistoryProvider(this);
        }

        /// <summary>
        /// Append additional history to internal storage.
        /// </summary>
        /// <param name="history">List of history entries to append.</param>
        /// <param name="periodSynchronizationToken">Token identifying time period from which historical data is from.</param>
        public void AppendHistory(List<OrderBook> history, int periodSynchronizationToken)
        {
            lock (_locker)
            {
                history.ForEach(entry => _history.AddLast(entry));
                _lastPeriodSynchronizationToken = periodSynchronizationToken;
            }
        }

        /// <summary>
        /// Removes next order book entry from history and returns it.
        /// </summary>
        /// <returns>Next order book entry from history.</returns>
        public OrderBook GetNext()
        {
            int historyCount;
            int? periodSynchronizationToken;
            lock (_locker)
            {
                historyCount = _history.Count;
                periodSynchronizationToken = _lastPeriodSynchronizationToken;
            }

            if (historyCount == 0)
            {
                if (!_loader.LoadData(periodSynchronizationToken))
                {
                    return null;
                }
            }

            lock (_locker)
            {
                var next = _history.First;
                _history.RemoveFirst();
                return next.Value;
            }
        }

        /// <summary>
        /// Closes queue.
        /// </summary>
        public void Close()
        {
            _loader.DeregisterHistoryProvider(this);
        }
    }
}
