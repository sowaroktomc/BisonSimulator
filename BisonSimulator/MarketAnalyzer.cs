using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim
{
    internal class MarketAnalyzer
    {
        private struct Entry
        {
            public Entry(DateTime time, decimal? askPrice, decimal? bidPrice)
            {
                Time = time;
                AskPrice = askPrice;
                BidPrice = bidPrice;
            }
            public DateTime Time { get; }
            public decimal? AskPrice { get; }
            public decimal? BidPrice { get; }
        }

        public class Statistic
        {
            private readonly List<decimal> _prices;
            private readonly Lazy<decimal> _lazyOpenPrice;
            private readonly Lazy<decimal> _lazyHighPrice;
            private readonly Lazy<decimal> _lazyLowPrice;
            private readonly Lazy<decimal> _lazyClosePrice;
            private readonly Lazy<decimal> _lazyVolatility;


            public Statistic(IEnumerable<decimal?> prices)
            {
                _prices = prices.Where(price => price.HasValue).Cast<decimal>().ToList();
                _lazyOpenPrice = new Lazy<decimal>(() => _prices.Count > 0 ? _prices[0] : 0);
                _lazyHighPrice = new Lazy<decimal>(() => _prices.Count > 0 ? _prices.Max() : 0);
                _lazyLowPrice = new Lazy<decimal>(() => _prices.Count > 0 ? _prices.Min() : 0);
                _lazyClosePrice = new Lazy<decimal>(() => _prices.Count > 0 ? _prices[_prices.Count - 1] : 0);
                _lazyVolatility = new Lazy<decimal>(CalcVolatility);
            }

            public decimal VolatilityValue => _lazyVolatility.Value;
            public decimal OpenPrice => _lazyOpenPrice.Value;

            public decimal ClosePrice => _lazyClosePrice.Value;
            public decimal HighPrice => _lazyHighPrice.Value;
            public decimal LowPrice => _lazyLowPrice.Value;


            private decimal CalcVolatility()
            {
                if (_prices.Count <= 1)
                {
                    return 0;
                }

                var mean = _prices.Average();
                var variance = _prices.Select(price => (price - mean) * (price - mean)).Sum() / (_prices.Count - 1);
                var standardDeviation = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(variance)));

                return standardDeviation / mean;
            }
        }

        private readonly LinkedList<Entry> _priceHistory;
        private readonly HistoryQueue _history;
        private DateTime _lastTimeStamp;
        private readonly Lazy<Statistic> _bidStat;
        private readonly Lazy<Statistic> _askStat;

        public TimeSpan MinTimespan { get; }

        public Statistic BidStat => _bidStat.Value;
        public Statistic AskStat => _askStat.Value;

        public MarketAnalyzer(BitstampHistoryLoader historyLoader)
        {
            MinTimespan = TimeSpan.FromMinutes(1);

            _bidStat = new Lazy<Statistic>(() => new Statistic(_priceHistory.Select(entry => entry.BidPrice)));
            _askStat = new Lazy<Statistic>(() => new Statistic(_priceHistory.Select(entry => entry.AskPrice)));

            _priceHistory = new LinkedList<Entry>();
            _history = new HistoryQueue(historyLoader);
        }

        public void LoadPeriod(DateTime fromTime, DateTime toTime)
        {
            if (toTime - fromTime < MinTimespan)
            {
                var adjustTicks = (MinTimespan.Ticks - toTime.Ticks + fromTime.Ticks) / 2;
                fromTime = fromTime.AddTicks(-adjustTicks);
                toTime = toTime.AddTicks(adjustTicks);
            }

            while (_lastTimeStamp < toTime)
            {
                var next = _history.GetNext();
                if (next != null)
                {
                    AppendOrderBook(next);
                }
                else
                {
                    break;
                }
            }

            while (_priceHistory.First.Value.Time < fromTime)
            {
                _priceHistory.RemoveFirst();
            }
        }
        
        private void AppendOrderBook(OrderBook book)
        {
            var lastEntry = _priceHistory.Last?.Value;
            var askPrice = book.Asks.Count > 0 ? book.Asks[0].Price : lastEntry?.AskPrice;
            var bidPrice = book.Bids.Count > 0 ? book.Bids[0].Price : lastEntry?.BidPrice;

            _priceHistory.AddLast(new Entry(book.AcqTime, askPrice, bidPrice));
            _lastTimeStamp = book.AcqTime;
        }

    }
}
