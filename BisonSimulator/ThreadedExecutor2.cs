using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sowalabs.Bison.ProfitSim
{
    internal class ThreadedExecutor2
    {
        private readonly List<Func<bool>> _tasks;
        private ConcurrentQueue<Func<bool>> _queue;
        private Thread[] _threads;
        private AutoResetEvent[] _resets;
        private const int ThreadCount = 8;
        Barrier _barrier = new Barrier(ThreadCount);
        private bool _isRunning = true;
        private CountdownEvent countdown;
        private bool haveMore;


        public ThreadedExecutor2(IEnumerable<Func<bool>> tasks)
        {
            _tasks = new List<Func<bool>>(tasks);
            _threads = Enumerable.Range(0, ThreadCount).Select(i => new Thread(Run)).ToArray();
            _resets = Enumerable.Range(0, ThreadCount).Select(i => new AutoResetEvent(false)).ToArray();

            countdown = new CountdownEvent(ThreadCount);
            Enumerable.Range(0, ThreadCount).ToList().ForEach(i => _threads[i].Start(_resets[i]));
        }

        private void Run(object reset)
        {

            while (_isRunning)
            {
                ((AutoResetEvent)reset).WaitOne();

                while (_queue.Count > 0)
                {
                    if (!_queue.TryDequeue(out var task))
                    {
                        continue;
                    }

                    var hasMore = task();
                    haveMore |= hasMore;
                    if (!hasMore)
                    {
                        lock (_tasks)
                        {
                            _tasks.Remove(task);
                        }
                    }
                }

                countdown.Signal();
            }
        }


        public bool ExecuteStep()
        {

            _queue = new ConcurrentQueue<Func<bool>>(_tasks);
            countdown.Reset();
            Enumerable.Range(0, ThreadCount).ToList().ForEach(i => _resets[i].Set());

            countdown.Wait();

            return haveMore;
        }

    }
}
