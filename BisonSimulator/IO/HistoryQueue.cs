using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    internal class HistoryQueue : IHistoryEnumerator
    {
        private readonly BitstampHistoryLoader _loader;
        private readonly List<OrderBook> _history = new List<OrderBook>();

        public HistoryQueue(BitstampHistoryLoader loader)
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
            if (_history.Count == 0)
            {
                if (!_loader.LoadData())
                {
                    return null;
                }
            }

            var next = _history[0];
            _history.RemoveAt(0);
            return next;
        }
    }
}
