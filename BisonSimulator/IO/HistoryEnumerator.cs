using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    internal class HistoryEnumerator : IHistoryEnumerator
    {
        private int _currentIndex;
        private readonly BitstampHistoryLoader _loader;

        private readonly List<OrderBook> _history = new List<OrderBook>();

        public HistoryEnumerator(BitstampHistoryLoader loader)
        {
            _loader = loader;
            _loader.RegisterEnumerator(this);
        }

        public void AppendHistory(List<OrderBook> history)
        {
            _history.AddRange(history);
        }

        public OrderBook GetNext()
        {
            if (_currentIndex >= _history.Count)
            {
                if (!_loader.LoadData())
                {
                    return null;
                }
            }

            return _history[_currentIndex++];
        }


        public bool Restart()
        {
            _history.RemoveAt(0);
            _currentIndex = 0;

            if (_history.Count == 0)
            {
                return _loader.LoadData();
            }

            return true;
        }
    }
}
