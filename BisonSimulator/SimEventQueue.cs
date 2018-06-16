using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim
{
    internal class SimEventQueue
    {
        private class QueueEntry
        {
            public DateTime TimeStamp { get; }
            public DateTime SimTime { get; }
            public int Priority { get; }
            public ISimEvent Event { get; }

            public QueueEntry(ISimEvent simEvent)
            {
                TimeStamp = DateTime.Now;
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
        
        private class QueueEntryComparer : IComparer<QueueEntry>
        {
            public int Compare(QueueEntry x, QueueEntry y)
            {
                Debug.Assert(x != null, nameof(x) + " != null");
                Debug.Assert(y != null, nameof(y) + " != null");

                if (x.Event == y.Event) return 0;
                if (x.SimTime != y.SimTime) return x.SimTime.CompareTo(y.SimTime);
                if (x.Priority != y.Priority) return x.Priority.CompareTo(y.Priority);
                if (x.TimeStamp != y.TimeStamp) return x.TimeStamp.CompareTo(y.TimeStamp);
                return x.Event.Id.CompareTo(y.Event.Id);
            }
        }

        private readonly SortedSet<QueueEntry> _queue = new SortedSet<QueueEntry>(new QueueEntryComparer());

        public void Enqueue(ISimEvent simEvent)
        {
            if (!_queue.Add(new QueueEntry(simEvent)))
            {
                throw new Exception("Duplicate event!?");
            }
        }

        public ISimEvent Dequeue()
        {
            var entry = _queue.Min;
            _queue.Remove(entry);
            return entry?.Event;
        }

        public void Remove(ISimEvent simEvent)
        {
            _queue.RemoveWhere(entry => entry.Event == simEvent);
        }

        public ISimEvent PeekLast()
        {
            return _queue.Max?.Event;
        }
    }
}
