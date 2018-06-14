using Sowalabs.Bison.LiquidityEngine.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sowalabs.Bison.LiquidityEngine
{


    public class Queue : IDisposable
    {
        protected static Queue SingletonInstance;
        private static readonly object SingletonPadlock = new object();

        public static Queue Instance
        {
            get
            {
                lock (SingletonPadlock)
                {
                    return SingletonInstance ?? (SingletonInstance = new Queue());
                }
            }
        }

        protected Queue()
        {
            this._tasks = new List<BaseTask>();
            this._workerBarrier = new ManualResetEventSlim(false);
            this._worker = new Thread(ExecuteTasks);
            this._worker.Start();
        }

        private readonly List<BaseTask> _tasks;
        private readonly ManualResetEventSlim _workerBarrier;
        private Thread _worker;
        private bool _isWorkerRunning;

        public virtual void Enqueue(BaseTask task)
        {
            this.AddTaskToQueue(task);
            this._workerBarrier.Set();
        }

        internal void AddTaskToQueue(BaseTask task)
        {
            lock (this._tasks)
            {
                this._tasks.Add(task);
            }
        }

        private void ExecuteTasks()
        {
            while (this._isWorkerRunning)
            {

                this._workerBarrier.Wait(1000);

                List<BaseTask> tasksToExecute;
                lock (_tasks)
                {
                    tasksToExecute = new List<BaseTask>(_tasks);
                    _tasks.Clear();
                }

                foreach (var task in tasksToExecute)
                {
                    if (!task.ShouldExecute())
                    {
                        this.AddTaskToQueue(task);
                    }
                    else
                    {
                        task.Execute();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._isWorkerRunning = false;
                this._workerBarrier.Set();
                this._worker?.Join();
                this._worker = null;
            }
        }
    }
}