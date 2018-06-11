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
                this.TimeStamp = DateTime.Now;
                this.Event = simEvent;
                this.SimTime = simEvent.SimTime;
                this.Priority = this.GetPriority(simEvent);
            }

            private int GetPriority(ISimEvent simEvent)
            {
                if (simEvent is OrderBookEvent)
                {
                    return 1;
                }
                else if (simEvent is NewPriceRequestEvent)
                {
                    return 2;
                }
                else
                {
                    return 10;
                }
            }

            public override string ToString()
            {
                return $"{this.SimTime:HH:mm:ss} {this.Event}";
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
            if (!this._queue.Add(new QueueEntry(simEvent)))
            {
                throw new Exception("Duplicate event!?");
            }
        }

        public ISimEvent Dequeue()
        {
            var entry = this._queue.Min;
            this._queue.Remove(entry);
            return entry?.Event;
        }

        public void Remove(ISimEvent simEvent)
        {
            this._queue.RemoveWhere(entry => entry.Event == simEvent);
        }

        public ISimEvent PeekLast()
        {
            return this._queue.Max?.Event;
        }
    }
}
