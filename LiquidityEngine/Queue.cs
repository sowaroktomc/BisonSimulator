using Sowalabs.Bison.LiquidityEngine.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Sowalabs.Bison.LiquidityEngine.Dependencies;

namespace Sowalabs.Bison.LiquidityEngine
{
    public class Queue : IQueue
    {
        internal Queue(IDependencyFactory dependencyFactory)
        {
            _tasks = new List<BaseTask>();
            _workerBarrier = dependencyFactory.CreateResetEvent();
            _isWorkerRunning = true;
            _worker = new Thread(ExecuteTasks);
            _worker.Start();
        }

        private readonly List<BaseTask> _tasks;
        private readonly IResetEvent _workerBarrier;
        private Thread _worker;
        private bool _isWorkerRunning;

        public virtual void Enqueue(BaseTask task)
        {
            ProcessTask(task);
        }

        internal void AddTaskToQueue(BaseTask task)
        {
            lock (_tasks)
            {
                _tasks.Add(task);
            }
        }

        private void ExecuteTasks()
        {
            while (_isWorkerRunning)
            {

                _workerBarrier.Wait(1000);

                List<BaseTask> tasksToExecute;
                lock (_tasks)
                {
                    tasksToExecute = new List<BaseTask>(_tasks);
                    _tasks.Clear();
                }

                foreach (var task in tasksToExecute)
                {
                    ProcessTask(task);
                }
            }
        }

        private void ProcessTask(BaseTask task)
        {
            if (!task.ShouldExecute())
            {
                AddTaskToQueue(task);
            }
            else
            {
                task.Execute();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _isWorkerRunning = false;
            _workerBarrier.Set();
            _worker?.Join();
            _worker = null;
            _workerBarrier.Dispose();
        }
    }
}