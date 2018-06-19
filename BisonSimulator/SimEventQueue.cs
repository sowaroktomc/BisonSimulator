using Sowalabs.Bison.ProfitSim.Events;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim
{
    internal class SimEventQueue
    {
        private class QueueEntry
        {
            public DateTime SimTime;
            public int Priority;
            public ISimEvent Event;

            public QueueEntry(ISimEvent simEvent)
            {
                Event = simEvent;
                SimTime = simEvent.SimTime;
                Priority = GetPriority(simEvent);
            }

            private int GetPriority(ISimEvent simEvent)
            {
                if (simEvent is OrderBookEvent)
                {
                    return 1;
                }

                if (simEvent is NewPriceRequestEvent)
                {
                    return 2;
                }

                if (simEvent is MarketTradeEvent)
                {
                    return 3;
                }

                return 10;
            }

            public override string ToString()
            {
                return $"{SimTime:HH:mm:ss} {Event}";
            }
        }
        
        private List<QueueEntry> _queue = new List<QueueEntry>();

        public void Enqueue(ISimEvent simEvent)
        {
            var min = 0;
            var max = _queue.Count;
            var newEntry = new QueueEntry(simEvent);

            while (min < max)
            {
                var mid = (max + min) / 2;
                var midEntry = _queue[mid];

                if (midEntry.Event == simEvent)
                {
                    return;
                }
                if (midEntry.SimTime < newEntry.SimTime)
                {
                    max = mid;
                    continue;
                }
                if (midEntry.SimTime == newEntry.SimTime && midEntry.Priority <= newEntry.Priority)
                {
                    max = mid;
                    continue;
                }
                min = mid + 1;
            }

            _queue.Insert(max, newEntry);
        }

        public ISimEvent Dequeue()
        {
            var count = _queue.Count;
            if (count == 0)
            {
                return null;
            }
            var entry = _queue[count - 1];
            _queue.RemoveAt(count - 1);
            return entry.Event;
        }

        public void Remove(ISimEvent simEvent)
        {
            _queue.RemoveAll(entry => entry.Event == simEvent);
        }

        public ISimEvent PeekLast()
        {
            if (_queue.Count == 0)
            {
                return null;
            }
            return _queue[0].Event;
        }
    }
}
