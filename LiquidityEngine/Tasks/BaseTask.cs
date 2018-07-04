using System;
using Sowalabs.Bison.LiquidityEngine.Dependencies;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    public abstract class BaseTask
    {
        protected enum ExecutionStatus
        {
            Done,
            Repeat,
            Error
        }
        private const int MaxRetryCount = 5;

        public DateTime ExecuteAtTime { get; private set; }
        private int _retryCount;
        protected LiquidityEngine Engine { get; }
        protected IDependencyFactory DependencyFactory { get; }

        protected BaseTask(IDependencyFactory dependencyFactory, LiquidityEngine engine)
        {
            ExecuteAtTime = dependencyFactory.DateTimeProvider.Now;
            DependencyFactory = dependencyFactory;
            Engine = engine;
        }

        protected abstract ExecutionStatus ExecuteTask();

        public bool ShouldExecute()
        {
            return DependencyFactory.DateTimeProvider.Now >= ExecuteAtTime;
        }

        public void Execute()
        {
            try
            {
                var executeSuccess = ExecuteTask();

                switch (executeSuccess)
                {
                    case ExecutionStatus.Done:
                        return;
                    case ExecutionStatus.Repeat:
                        ExecuteAtTime = DependencyFactory.DateTimeProvider.Now.AddSeconds(10);
                        Engine.Queue.Enqueue(this);
                        return;
                    case ExecutionStatus.Error:
                        _retryCount++;
                        ExecuteAtTime = DependencyFactory.DateTimeProvider.Now.AddSeconds(_retryCount);

                        if (_retryCount > MaxRetryCount)
                        {
                            // TODO Notify someone!
                            return;
                        }

                        Engine.Queue.Enqueue(this);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            }
            catch
            {
                //TODO Logging!
            }
        }
    }
}