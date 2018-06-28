using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// Executes steps on several threads until all steps are done.
    /// </summary>
    internal class ThreadedExecutor
    {
        private readonly List<Func<bool>> _steps;
        private readonly int _maxTaskCount = Math.Max(Environment.ProcessorCount - 1, 1);

        /// <summary>
        /// Executes given steps on several threads until all steps are done.
        /// </summary>
        /// <param name="steps">List of steps to execute.</param>
        public ThreadedExecutor(IEnumerable<Func<bool>> steps)
        {
            _steps = new List<Func<bool>>(steps);
            Trace.WriteLine($"Max {_maxTaskCount} worker threads available.");
        }

        /// <summary>
        /// Executes all steps in several threads and returns true if any step has more to do. Each step should return whether it is done or not.
        /// </summary>
        /// <returns>True if any step has more to do. False otherwise.</returns>
        public bool ExecuteSteps()
        {
            var semaphore = new SemaphoreSlim(_maxTaskCount);
            var trackedTasks = new List<Task>();
            var haveMore = false;

            // ReSharper disable once InconsistentlySynchronizedField - Locking on _steps not needed at this point as no other threads are running jet!
            var queue = new ConcurrentQueue<Func<bool>>(_steps);

            while (!queue.IsEmpty)
            {
                semaphore.Wait();
                trackedTasks.Add(Task.Run(() =>
                {
                    try
                    {
                        if (!queue.TryDequeue(out var step))
                        {
                            return;
                        }

                        var hasMore = step();
                        haveMore |= hasMore;

                        // Remove step from list if it is done with its work.
                        if (!hasMore)
                        {
                            // Lock. Other tasks might also be removing a step just now.
                            lock (_steps)
                            {
                                _steps.Remove(step);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"{ex} {ex.StackTrace}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            Task.WaitAll(trackedTasks.ToArray());

            return haveMore;
        }
    }
}
