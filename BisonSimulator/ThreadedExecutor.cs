using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sowalabs.Bison.ProfitSim
{
    internal class ThreadedExecutor 
    {


        private readonly List<Func<bool>> _tasks;
        

        public ThreadedExecutor(IEnumerable<Func<bool>> tasks)
        {
            _tasks = new List<Func<bool>>(tasks);
        }

        public bool ExecuteStep()
        {

            var queue = new ConcurrentQueue<Func<bool>>(_tasks);
            var semaphore = new SemaphoreSlim(10);
            var trackedTasks = new List<Task>();
            var haveMore = false;

            while (!queue.IsEmpty)
            {
                semaphore.Wait();
                trackedTasks.Add(Task.Run(() =>
                {
                    if (!queue.TryDequeue(out var task))
                    {
                        return;
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

                    semaphore.Release();
                }));
            }
            Task.WaitAll(trackedTasks.ToArray());

            return haveMore;
        }
    }
}
