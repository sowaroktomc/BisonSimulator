using Sowalabs.Bison.ProfitSim.Events;
using System;
using System.Collections.Generic;
// ReSharper disable FieldCanBeMadeReadOnly.Local -> fields are not made readonly as a speed optimization.


namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// Queue of simulation events.
    /// </summary>
    /// <remarks>Qeueue is implemented with a list where events are ordered in descending order - next event is on last position in list and last event is first in list.
    /// Events are ordered first by time and then priority (each simulation event type has a distinct priority). </remarks>
    internal class SimEventQueue
    {
        private class QueueEntry
        {
            internal DateTime SimTime;
            internal int Priority;
            internal ISimEvent Event;

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

        /// <summary>
        /// Enqueues given event into queue.
        /// </summary>
        /// <param name="simEvent">Event to be enqueued into queue.</param>
        public void Enqueue(ISimEvent simEvent)
        {

            // Event is positioned using bisection.

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

        /// <summary>
        /// Dequeues next event from queue.
        /// </summary>
        /// <returns>Event next in queue.</returns>
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

        /// <summary>
        /// Removes event from queue.
        /// </summary>
        /// <param name="simEvent">Event to be removed from queue.</param>
        public void Remove(ISimEvent simEvent)
        {
            _queue.RemoveAll(entry => entry.Event == simEvent);
        }

        /// <summary>
        /// Returns last event in queue.
        /// </summary>
        /// <returns>Last event in queue.</returns>
        public ISimEvent PeekLast()
        {
            return _queue.Count == 0 ? null : _queue[0].Event;
        }
    }
}
