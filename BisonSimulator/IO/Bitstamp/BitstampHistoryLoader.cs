﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp
{
    internal class BitstampHistoryLoader
    {
        private readonly List<IHistoryEnumerator> _enumerators = new List<IHistoryEnumerator>();

        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private DateTime _currentDate;
        private static object _synchronizationContext = new object();
        public object SynchronizationContext {  get { return _synchronizationContext; } }

        public Crypto Crypto { get; }

        public BitstampHistoryLoader(Crypto crypto, DateTime? fromDateTime, DateTime? toDateTime)
        {
            Crypto = crypto;
            _fromDate = fromDateTime ?? new DateTime(2018, 4, 4, 0, 0, 0);
            _toDate = toDateTime ?? new DateTime(2018, 5, 1, 0, 0, 0);
            _currentDate = _fromDate;
        }

        public void Restart()
        {
            lock (SynchronizationContext)
            {
                _currentDate = _fromDate;
            }
        }

        private string GetAddress()
        {
            return $"http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_{Crypto}eur_{_currentDate.Year:D4}/orders_{_currentDate.Year:D4}_{_currentDate.Month:D2}_{_currentDate.Day:D2}_{_currentDate.Hour:D2}";
        }

        public bool LoadData()
        {
            lock (SynchronizationContext)
            {
                if (_currentDate > _toDate)
                {
                    return false;
                }

                var orderBookHistory = new List<Common.Trading.OrderBook>();

                using (var webClient = new WebClient())
                {
                    var address = GetAddress();
                    Trace.Write("Loading data from " + address);

                    using (var reader = new JsonTextReader(new StreamReader(webClient.OpenRead(address))))
                    {
                        reader.SupportMultipleContent = true;
                        var serializer = new JsonSerializer();
                        while (reader.Read())
                        {
                            reader.Read(); //Each JSON is prefixed with a timestamp value - skip it.
                            orderBookHistory.Add(Convert(serializer.Deserialize<OrderBook>(reader)));
                        }
                    }
                    Trace.WriteLine("   -   done.");
                }

                _enumerators.ForEach(enumerator => enumerator.AppendHistory(orderBookHistory));
                _currentDate = _currentDate.AddHours(1);
                return true;
            }
        }

        public void RegisterEnumerator(IHistoryEnumerator enumerator)
        {
            if (_currentDate > _fromDate)
            {
                throw new Exception("Adding data enumerator after data has been loaded.");
            }
            _enumerators.Add(enumerator);
        }

        private Common.Trading.OrderBook Convert(OrderBook bitstampBook)
        {
            return new Common.Trading.OrderBook
            {
                AcqTime = new DateTime(bitstampBook.AcqTime.Ticks - bitstampBook.AcqTime.Ticks % TimeSpan.TicksPerSecond), // Time on the seconds resolution.
                Asks = new List<Common.Trading.OrderBookEntry>(bitstampBook.Asks.Select(Convert)),
                Bids = new List<Common.Trading.OrderBookEntry>(bitstampBook.Bids.Select(Convert))
            };
        }

        private static Common.Trading.OrderBookEntry Convert(Offer bitstampOffer)
        {
            return new Common.Trading.OrderBookEntry { Amount = bitstampOffer.Order.Amount, Price = bitstampOffer.Order.Price};
        }
    }
}
