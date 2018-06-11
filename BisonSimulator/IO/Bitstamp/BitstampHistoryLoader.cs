using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model;
using OrderBook = Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model.OrderBook;

namespace Sowalabs.Bison.ProfitSim.IO.Bitstamp
{
    internal class BitstampHistoryLoader
    {
        private List<Common.Trading.OrderBook> _orderBookHistory;
        private Queue<string> _addressQueue = new Queue<string>();

        public BitstampHistoryLoader()
        {
            this._orderBookHistory = new List<Common.Trading.OrderBook>();

            //this._addressQueue.Enqueue(@"C:\Sowalabs\BisonSimulator\History.json");
            //this._addressQueue.Enqueue(@"C:\Sowalabs\BisonSimulator\History2.json");
            this._addressQueue.Enqueue("http://api.sowalabs.com/cryptotickernest/api/get/bitstamp_btceur_2018/orders_2018_06_06_09");

            this.LoadData();
        }

        private bool LoadData()
        {
            if (this._addressQueue.Count == 0)
            {
                return false;
            }

            var orderBookHistory = new List<OrderBook>();

            using (var webClient = new WebClient())
            {
//                using (var reader = new JsonTextReader(new StreamReader(new FileStream(this._addressQueue.Dequeue(), FileMode.Open))))
                using (var reader = new JsonTextReader(new StreamReader(webClient.OpenRead(this._addressQueue.Dequeue()))))
                {
                    reader.SupportMultipleContent = true;
                    var serializer = new JsonSerializer();
                    while (reader.Read())
                    {
                        reader.Read(); //Each JSON is prefixed with a timestamp value - skip it.
                        orderBookHistory.Add(serializer.Deserialize<OrderBook>(reader));
                    }
                }
            }

            this._orderBookHistory.AddRange(orderBookHistory.Select(Convert));

            this._historyEnumerator = this._orderBookHistory.GetEnumerator();

            return true;
        }

        private IEnumerator<Common.Trading.OrderBook> _historyEnumerator;

        public bool Restart()
        {
            this._orderBookHistory.RemoveAt(0);

            if (this._orderBookHistory.Count == 0)
            {
                return this.LoadData();
            }

            this._historyEnumerator = this._orderBookHistory.GetEnumerator();
            return true;
        }

        public Common.Trading.OrderBook GetNext()
        {
            if (!this._historyEnumerator.MoveNext())
            {
                var oldEntriesCount = this._orderBookHistory.Count;
                if (!this.LoadData())
                {
                    return null;
                }

                // position on first entry.
                this._historyEnumerator.MoveNext();

                // Skip all entries in collection before new entries were loaded -> position on first new entry.
                for (int i = 0; i < oldEntriesCount; i++)
                {
                    this._historyEnumerator.MoveNext();
                }

            }
            return this._historyEnumerator.Current;
        }

        private Common.Trading.OrderBook Convert(OrderBook bitstampBook)
        {
            return new Common.Trading.OrderBook
            {
                AcqTime = new DateTime(bitstampBook.AcqTime.Ticks - (bitstampBook.AcqTime.Ticks % TimeSpan.TicksPerSecond)), // Time on the seconds resolution.
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
