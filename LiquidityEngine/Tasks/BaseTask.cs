using System;
using Sowalabs.Bison.LiquidityEngine.Dependencies;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    public abstract class BaseTask
    {
        private const int MaxRetryCount = 5;

        private DateTime _executeAtTime;
        private int _retryCount;
        protected IDependencyFactory DependencyFactory { get; }

        protected BaseTask(IDependencyFactory dependencyFactory)
        {
            this._executeAtTime = DateTime.Now;
            this.DependencyFactory = dependencyFactory;
        }

        protected abstract bool ExecuteTask();

        public bool ShouldExecute()
        {
            return DateTime.Now >= this._executeAtTime;
        }

        public void Execute()
        {
            try
            {
                var executeSuccess = this.ExecuteTask();

                if (executeSuccess)
                {
                    return;
                }

                this._retryCount++;
                _executeAtTime = DateTime.Now.AddSeconds(this._retryCount);

                if (this._retryCount > MaxRetryCount)
                {
                    // TODO Notify someone!
                    return;
                }

                Queue.Instance.AddTaskToQueue(this);
            }
            catch
            {
                //TODO Logging!
            }
        }
    }
}