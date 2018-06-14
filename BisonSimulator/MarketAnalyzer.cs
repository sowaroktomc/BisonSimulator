using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Sowalabs.Bison.ProfitSim.IO;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;

namespace Sowalabs.Bison.ProfitSim
{
    class MarketAnalyzer
    {
        private struct Entry
        {
            public Entry(DateTime time, decimal askPrice, decimal bidPrice)
            {
                Time = time;
                AskPrice = askPrice;
                BidPrice = bidPrice;
            }
            public DateTime Time { get; }
            public decimal AskPrice { get; }
            public decimal BidPrice { get; }
            public decimal Price => (AskPrice + BidPrice) / 2;
        }

        public MarketVolatility Volatility
        {
            get
            {
                if (_priceHistory.Count <= 1)
                {
                    return MarketVolatility.Nonvolatile;
                }

                var mean = _priceHistory.Average(entry => entry.Price);
                var variance = _priceHistory.Select(entry => (entry.Price - mean) * (entry.Price - mean)).Sum() / (_priceHistory.Count - 1);
                var standardDeviation = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(variance)));

                return standardDeviation / mean > 0.05m ? MarketVolatility.Volatile : MarketVolatility.Nonvolatile;
            }
        }
        public MarketTrend Trend
        {
            get
            {
                var priceDiff = _priceHistory[_priceHistory.Count - 1].Price - _priceHistory[0].Price;
                if (priceDiff < 0)
                {
                    return MarketTrend.Down;
                }

                if (priceDiff == 0)
                {
                    return MarketTrend.NoChange;
                }

                return MarketTrend.Up;
            }
        }
        public MarketExtreme Extreme
        {
            get
            {
                var priceDiff = (_priceHistory[_priceHistory.Count - 1].Price - _priceHistory[0].Price) / _priceHistory[0].Price;
                if (priceDiff <= -0.02m)
                {
                    return MarketExtreme.HighFall;
                }
                if (priceDiff >= 0.02m)
                {
                    return MarketExtreme.HighRise;
                }

                return MarketExtreme.None;
            }
        }


        private readonly List<Entry> _priceHistory;
        private readonly HistoryQueue _history;

        public TimeSpan MaxTimespan { get; }

        public MarketAnalyzer(HistoryQueue historyQueue)
        {
            _priceHistory = new List<Entry>();
            _history = historyQueue;
            MaxTimespan = TimeSpan.FromHours(1);
            var halfMaxTimespan = new TimeSpan(MaxTimespan.Ticks / 2);

            var first = _history.GetNext();
            AppendOrderBook(first);
            for (var next = _history.GetNext(); next != null && next.AcqTime < first.AcqTime.Add(halfMaxTimespan); next = _history.GetNext())
            {
                AppendOrderBook(next);
            }
        }

        public void Next()
        {
            var next = _history.GetNext();
            if (next != null)
            {
                AppendOrderBook(next);

                while (_priceHistory[_priceHistory.Count - 1].Time - _priceHistory[0].Time > MaxTimespan)
                {
                    _priceHistory.RemoveAt(0);
                }
            }
            else
            {
                _priceHistory.RemoveAt(0);
            }
        }

        private void AppendOrderBook(OrderBook book)
        {
            if (_priceHistory.Count == 0 && (book.Asks.Count == 0 || book.Bids.Count == 0))
            {
                return;
            }

            var askPrice = book.Asks.Count > 0 ? book.Asks[0].Price : _priceHistory[_priceHistory.Count - 1].AskPrice;
            var bidPrice = book.Bids.Count > 0 ? book.Bids[0].Price : _priceHistory[_priceHistory.Count - 1].BidPrice;

            _priceHistory.Add(new Entry(book.AcqTime, askPrice, bidPrice));
        }

    }
}
