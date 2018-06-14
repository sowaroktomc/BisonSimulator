using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp
{
    internal class BitstampHistoryLoader
    {
        private readonly Queue<string> _addressQueue = new Queue<string>();
        private readonly List<IHistoryEnumerator> _enumerators = new List<IHistoryEnumerator>();
        private bool _firstDataLoaded;

        public BitstampHistoryLoader()
        {
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_00");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_01");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_02");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_03");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_04");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_05");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_06");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_07");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_08");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_09");
            _addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_10");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_11");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_12");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_13");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_14");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_15");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_16");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_17");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_18");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_19");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_20");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_21");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_22");
            //_addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_23");
        }

        public bool LoadData()
        {
            if (_addressQueue.Count == 0)
            {
                return false;
            }

            var orderBookHistory = new List<Common.Trading.OrderBook>();
            
            using (var webClient = new WebClient())
            {
                var address = _addressQueue.Dequeue();
                System.Diagnostics.Debug.Write("Loading data from " + address);

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
                System.Diagnostics.Debug.WriteLine("   -   done.");
            }

            _enumerators.ForEach(enumerator => enumerator.AppendHistory(orderBookHistory));
            _firstDataLoaded = true;
            return true;
        }

        public void RegisterEnumerator(IHistoryEnumerator enumerator)
        {
            if (_firstDataLoaded)
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
