using Newtonsoft.Json;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp
{
    /// <summary>
    /// Loads Bitstamp order book history from web. Data is loaded in sections.
    /// </summary>
    internal class BitstampHistoryLoader
    {
        /// <summary>
        /// Crypto-currency the loader is loading data for.
        /// </summary>
        public Crypto Crypto { get; }

        private readonly List<IHistoryProvider> _loaders = new List<IHistoryProvider>();
        private readonly DateTime _toDateTime;
        private readonly object _locker = new object();

        private DateTime _nextDateTimeToLoad;
        private int _lastPeriodSynchronizationToken;


        /// <summary>
        /// Loads Bitstamp order book history from web. Data is loaded in sections.
        /// </summary>
        /// <param name="crypto">Crypto-currency the loader will load data for.</param>
        /// <param name="fromDateTime">Date and time from when the history will be loaded from.</param>
        /// <param name="toDateTime">Date and time to when the history will be loaded to.</param>
        public BitstampHistoryLoader(Crypto crypto, DateTime fromDateTime, DateTime toDateTime)
        {
            if (fromDateTime == DateTime.MinValue)
            {
                throw new ArgumentNullException(nameof(fromDateTime));
            }
            if (toDateTime == DateTime.MinValue)
            {
                throw new ArgumentNullException(nameof(toDateTime));
            }

            Crypto = crypto;

            // As data from exchange is stored by hours, from and to times are floored to hour part.
            _nextDateTimeToLoad = fromDateTime.Date.AddHours(fromDateTime.Hour);
            _toDateTime = toDateTime.Date.AddHours(toDateTime.Hour);
        }

        /// <summary>
        /// Load new data from source. Data will only be loaded if given synchronization token is valid (last).
        /// </summary>
        /// <param name="periodSynchronizationToken">Token with which caller identifies its current history state. Data is only loaded if the state is most current.</param>
        /// <returns>True if data has been loaded. False otherwise (no more data available).</returns>
        public bool LoadData(int? periodSynchronizationToken)
        {
            lock (_locker)
            {
                if ((periodSynchronizationToken + 1 ?? 1) <= _lastPeriodSynchronizationToken)
                {
                    // Period has already been loaded -> return a successfull loading.
                    return true;
                }

                if (_nextDateTimeToLoad > _toDateTime)
                {
                    return false;
                }

                var filename = GetNextCachedFilename();

                if (!File.Exists(filename) || new FileInfo(filename).Length == 0)
                {

                    if (!Directory.Exists(".\\Cache"))
                    {
                        Directory.CreateDirectory(".\\Cache");
                    }

                    StoreDataToCacheFile(filename);
                }

                // Sometimes there is no data in source -> the loaded data is an exception description.
                if (new FileInfo(filename).Length < 1000000)
                {
                    Trace.WriteLine($"{Crypto} error in data detected, skipping period!");
                    _nextDateTimeToLoad = _nextDateTimeToLoad.AddHours(1);
                    return LoadData(periodSynchronizationToken);
                }


                var orderBookHistory = LoadOrderbooksFromFile(filename);

                _lastPeriodSynchronizationToken++;
                _loaders.ForEach(enumerator => enumerator.AppendHistory(orderBookHistory, _lastPeriodSynchronizationToken));
                _nextDateTimeToLoad = _nextDateTimeToLoad.AddHours(1);
                return true;
            }
        }

        /// <summary>
        /// Loads orderbook historical data from given file.
        /// </summary>
        /// <param name="filename">Name of file to load orderbook historical data from.</param>
        /// <returns>List of loaded orderbooks.</returns>
        private List<Common.Trading.OrderBook> LoadOrderbooksFromFile(string filename)
        {
            using (var reader = new JsonTextReader(new StreamReader(filename)))
            {
                var orderBookHistory = new List<Common.Trading.OrderBook>();
                Trace.WriteLine("Loading data from " + filename);
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                while (reader.Read())
                {
                    reader.Read(); //Each JSON is prefixed with a timestamp value - skip it.
                    orderBookHistory.Add(Convert(serializer.Deserialize<OrderBook>(reader)));
                }

                Trace.WriteLine($"{Crypto} loading done.");

                return orderBookHistory;
            }
        }

        /// <summary>
        /// Downloads orderbook historical data from web and stores it into given file.
        /// </summary>
        /// <param name="filename">Name of file to store data into.</param>
        private void StoreDataToCacheFile(string filename)
        {
            using (var webClient = new WebClient())
            {
                var address = GetNextWebAddress();
                Trace.WriteLine("Downloading data from " + address);
                webClient.DownloadFile(address, filename);
                Trace.WriteLine($"{Crypto} downloading done.");
            }
        }

        /// <summary>
        /// Registers given history provider to recieve all future loaded data.
        /// </summary>
        /// <param name="provider">Provider to recieve all future loaded data.</param>
        public void RegisterHistoryProvider(IHistoryProvider provider)
        {
            lock (_locker)
            {
                if (_lastPeriodSynchronizationToken > 0)
                {
                    throw new Exception("Adding data provider after data has been loaded.");
                }

                _loaders.Add(provider);
            }
        }

        /// <summary>
        /// Deregisters given history provider from recieving any future loaded data.
        /// </summary>
        /// <param name="provider">Provider which stops recieving all future loaded data.</param>
        public void DeregisterHistoryProvider(IHistoryProvider provider)
        {
            lock (_locker)
            {
                _loaders.Remove(provider);
            }
        }


        /// <summary>
        /// Converts bistamp orderbook data into Bison data.
        /// </summary>
        private Common.Trading.OrderBook Convert(OrderBook bitstampBook)
        {
            return new Common.Trading.OrderBook
            {
                AcqTime = new DateTime(bitstampBook.AcqTime.Ticks - bitstampBook.AcqTime.Ticks % TimeSpan.TicksPerSecond), // Time on the seconds resolution.
                Asks = new List<Common.Trading.OrderBookEntry>(bitstampBook.Asks.Select(Convert)),
                Bids = new List<Common.Trading.OrderBookEntry>(bitstampBook.Bids.Select(Convert))
            };
        }

        /// <summary>
        /// Converts bistamp orderbook entry into Bison data.
        /// </summary>
        private static Common.Trading.OrderBookEntry Convert(Offer bitstampOffer)
        {
            return new Common.Trading.OrderBookEntry { Amount = bitstampOffer.Order.Amount, Price = bitstampOffer.Order.Price};
        }

        private string GetNextWebAddress()
        {
            return $"http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_{Crypto}eur_{_nextDateTimeToLoad.Year:D4}/orders_{_nextDateTimeToLoad.Year:D4}_{_nextDateTimeToLoad.Month:D2}_{_nextDateTimeToLoad.Day:D2}_{_nextDateTimeToLoad.Hour:D2}";
        }

        private string GetNextCachedFilename()
        {
            return $".\\Cache\\bitstamp_{Crypto}eur_orders_{_nextDateTimeToLoad.Year:D4}_{_nextDateTimeToLoad.Month:D2}_{_nextDateTimeToLoad.Day:D2}_{_nextDateTimeToLoad.Hour:D2}";
        }
    }
}
