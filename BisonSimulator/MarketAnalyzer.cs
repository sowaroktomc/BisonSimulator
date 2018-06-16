using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.IO;
using Sowalabs.Bison.ProfitSim.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;

namespace Sowalabs.Bison.ProfitSim
{
    class MarketAnalyzer
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
            public Statistic(IEnumerable<decimal?> prices)
            {
                _prices = prices.Where(price => price.HasValue).Cast<decimal>().ToList();
            }


            public Lazy<MarketVolatility> Volatility => new Lazy<MarketVolatility>(() =>
                {
                    if (_prices.Count <= 1)
                    {
                        return MarketVolatility.Nonvolatile;
                    }


                    return VolatilityValue.Value > 0.01m ? MarketVolatility.Volatile : MarketVolatility.Nonvolatile;
                }
            );

            public Lazy<decimal> VolatilityValue => new Lazy<decimal>(() =>
            {
                if (_prices.Count <= 1)
                {
                    return 0;
                }

                var mean = _prices.Average();
                var variance = _prices.Select(price => (price - mean) * (price - mean)).Sum() / (_prices.Count - 1);
                var standardDeviation = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(variance)));

                return standardDeviation / mean;

            });

            public Lazy<decimal> OpenPrice => new Lazy<decimal>(() => _prices.Count > 0 ? _prices[0] : 0);
            public Lazy<decimal> ClosePrice => new Lazy<decimal>(() => _prices.Count > 0 ? _prices[_prices.Count - 1] : 0);
            public Lazy<decimal> HighPrice => new Lazy<decimal>(() => _prices.Count > 0 ? _prices.Max() : 0);
            public Lazy<decimal> LowPrice => new Lazy<decimal>(() => _prices.Count > 0 ? _prices.Min() : 0);

            public Lazy<MarketTrend> Trend => new Lazy<MarketTrend>(() =>
            {
                var priceDiff = _prices[_prices.Count - 1] - _prices[0];
                if (priceDiff < 0)
                {
                    return MarketTrend.Down;
                }

                if (priceDiff == 0)
                {
                    return MarketTrend.NoChange;
                }

                return MarketTrend.Up;
            });

            public Lazy<MarketExtreme> Extreme => new Lazy<MarketExtreme>(() =>
            {
                var priceDiff = (_prices[_prices.Count - 1] - _prices[0]) / _prices[0];
                if (priceDiff <= -0.02m)
                {
                    return MarketExtreme.HighFall;
                }

                if (priceDiff >= 0.02m)
                {
                    return MarketExtreme.HighRise;
                }

                return MarketExtreme.None;
            });
        }


        private List<Entry> _priceHistory;
        private HistoryQueue _history;
        private readonly BitstampHistoryLoader _historyLoader;

        public TimeSpan MaxTimespan { get; }
        public Statistic BidStat { get; private set; }
        public Statistic AskStat { get; private set; }

        public MarketAnalyzer(BitstampHistoryLoader historyLoader)
        {
            MaxTimespan = TimeSpan.FromSeconds(20);

            _historyLoader = historyLoader;
        }

        public void Init()
        {
            _priceHistory = new List<Entry>();
            _history = new HistoryQueue(_historyLoader);

            var halfMaxTimespan = new TimeSpan(MaxTimespan.Ticks / 2);

            var first = _history.GetNext();
            AppendOrderBook(first);
            for (var next = _history.GetNext(); next != null && next.AcqTime < first.AcqTime.Add(halfMaxTimespan); next = _history.GetNext())
            {
                AppendOrderBook(next);
            }

            CreateStatistics();
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
                if (_priceHistory.Count > 0)
                {
                    _priceHistory.RemoveAt(0);
                }
            }

            CreateStatistics();
        }

        private void CreateStatistics()
        {
            BidStat = new Statistic(_priceHistory.Select(entry => entry.BidPrice));
            AskStat = new Statistic(_priceHistory.Select(entry => entry.AskPrice));
        }

        private void AppendOrderBook(OrderBook book)
        {
            var lastEntry = _priceHistory.Count > 0 ? _priceHistory[_priceHistory.Count - 1] : (Entry?) null;
            var askPrice = book.Asks.Count > 0 ? book.Asks[0].Price : lastEntry?.AskPrice;
            var bidPrice = book.Bids.Count > 0 ? book.Bids[0].Price : lastEntry?.BidPrice;

            _priceHistory.Add(new Entry(book.AcqTime, askPrice, bidPrice));
        }

    }
}
