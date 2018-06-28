using Sowalabs.Bison.ProfitSim.Events;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim
{
    internal class SimEventQueue
    {

        public class MyList<T>
        {
            private T[] _store = new T[10000];
            private int _firstIndex = 0;
            private int _lastIndex = -1;

            public T this[int index]
            {
                get { return _store[_firstIndex + index]; }
            }
            public T RemoveFirst()
            {
                return _store[_firstIndex++];
            }

            public void Insert(int index, T value)
            {
                if (_firstIndex > _lastIndex)
                {
                    _firstIndex = 0;
                    _lastIndex = 0;
                    _store[0] = value;
                    return;
                }
                if (index > _lastIndex)
                {
                    _store[++_lastIndex] = value;
                    return;
                }

                var oldArray = _store;
                _store = new T[10000];
                Array.Copy(oldArray, _firstIndex, _store, 0, index);
                _store[index] = value;
                Array.Copy(oldArray, _firstIndex + index, _store, index + 1, _lastIndex - index - _firstIndex + 1);
                _lastIndex = _lastIndex - _firstIndex + 1;
                _firstIndex = 0;
            }

            public void RemoveAt(int index)
            {
                Array.Copy(_store, index + 1, _store, index, _lastIndex - index);
                _lastIndex--;
            }

            public void RemoveAll(Func<T, bool> predicate)
            {
                var index = 0;
                while (index < Count)
                {
                    if (predicate(_store[_firstIndex + index]))
                    {
                        RemoveAt(index);
                    } else
                    {
                        index++;
                    }
                }
            }

            public int Count
            {
                get { return _lastIndex - _firstIndex + 1; }
            }
        }

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
        
        private MyList<QueueEntry> _queue = new MyList<QueueEntry>();

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
                    min = mid + 1;
                    continue;
                }
                if (midEntry.SimTime == newEntry.SimTime && midEntry.Priority <= newEntry.Priority)
                {
                    min = mid + 1;
                    continue;
                }
                max = mid;
            }

            _queue.Insert(min, newEntry);
        }

        public ISimEvent Dequeue()
        {
            var count = _queue.Count;
            if (count == 0)
            {
                return null;
            }
            return _queue.RemoveFirst().Event;
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
            return _queue[_queue.Count - 1].Event;
        }
    }
}
