using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    /// <summary>
    /// Iterates multiple times over order book history and loads new data when needed.
    /// </summary>
    internal class HistoryEnumerator : IHistoryProvider
    {
        private LinkedListNode<OrderBook> _currentNode;
        private readonly BitstampHistoryLoader _loader;
        private readonly object _locker = new object();
        private int? _lastPeriodSynchronizationToken;
        private readonly LinkedList<OrderBook> _history = new LinkedList<OrderBook>();


        /// <summary>
        /// Iterates over order book history and loads new data using given history loader when needed.
        /// </summary>
        /// <param name="loader">Order book history loader which is then used to load historical data.</param>
        public HistoryEnumerator(BitstampHistoryLoader loader)
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
        /// Moves iterator to next entry and return next order book entry from history. If no more historical data is available, returns null.
        /// </summary>
        /// <returns>Next order book entry or null if no more is available.</returns>
        public OrderBook GetNext()
        {
            LinkedListNode<OrderBook> nextNode;
            int? periodSynchronizationToken;
            lock (_locker)
            {
                nextNode = _currentNode != null ? _currentNode.Next : _history.First;
                periodSynchronizationToken = _lastPeriodSynchronizationToken;
            }

            if (nextNode == null)
            {
                if (!_loader.LoadData(periodSynchronizationToken))
                {
                    return null;
                }
            }

            lock (_locker)
            {
                _currentNode = _currentNode != null ? _currentNode.Next : _history.First;
                return _currentNode?.Value;
            }
        }
        
        /// <summary>
        /// Removes first entry from storage, restarts history iterator and returns whether any more history entries are available.
        /// </summary>
        /// <returns>True if more history entries are available. False othervise.</returns>
        public bool Restart()
        {
            int historyCount;
            int? periodSynchronizationToken;

            lock (_locker)
            {
                _history.RemoveFirst();
                _currentNode = null;

                historyCount = _history.Count;
                periodSynchronizationToken = _lastPeriodSynchronizationToken;
            }

            if (historyCount == 0)
            {
                return _loader.LoadData(periodSynchronizationToken);
            }

            return true;
        }

        /// <summary>
        /// Closes enumerator.
        /// </summary>
        public void Close()
        {
            _loader.DeregisterHistoryProvider(this);
        }
    }
}
