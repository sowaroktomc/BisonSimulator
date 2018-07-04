using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Sowalabs.Bison.Data;
using Sowalabs.Bison.Data.Types;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.WhenStrategies;

namespace Sowalabs.Bison.LiquidityEngine.TaskGenerators
{
    public class UserMoneyTransferGenerator : DelayedExecutionStrategy.IDelayedExecutionStrategyConsumer, ITaskGenerator, IDisposable
    {
        private readonly object _locker = new object();
        private readonly List<IWhenStrategy> _whenStrategies;
        private readonly List<UserMoneyTransfer> _openTransfers = new List<UserMoneyTransfer>();
        private readonly DataAccessLayer.UserMoneyTransfer _dalTransfer = new DataAccessLayer.UserMoneyTransfer();
        private readonly DataAccessLayer.UserMoneyTransferTask _dalTask = new DataAccessLayer.UserMoneyTransferTask();
        private readonly IDependencyFactory _dependencyFactory;
        private readonly LiquidityEngine _engine;

        public int Delay { get; set; }

        public event EventHandler OnExecute;

        public UserMoneyTransferGenerator(IDependencyFactory dependencyFactory, LiquidityEngine engine)
        {
            _dependencyFactory = dependencyFactory;
            _engine = engine;
            _whenStrategies = new List<IWhenStrategy>(new[] {new DelayedExecutionStrategy(this, dependencyFactory)});
        }

        public void Initialize()
        {
            var openTasksData = _dalTask.GetOpenTasks();
            openTasksData.ForEach(taskData => new Tasks.UserMoneyTransferTask(_dependencyFactory, _engine, taskData).Enqueue());

            _openTransfers.AddRange(_dalTransfer.GetNewTransfers());
            Execute();
        }

        // TODO Kill generator if any part of execution fails?
        public void Execute()
        {
            List<UserMoneyTransfer> transfersToExecute;
            lock (_locker)
            {
                transfersToExecute = new List<UserMoneyTransfer>(_openTransfers);
                _openTransfers.Clear();

                OnExecute?.Invoke(this, new EventArgs());
            }


            var groups = transfersToExecute.GroupBy(transfer => new {transfer.UserId, transfer.UserAccount, transfer.Currency}).ToList();
            foreach (var group in groups)
            {
                var taskData = new UserMoneyTransferTask
                {
                    Amount = group.Sum(transfer => transfer.Amount),
                    Currency = group.Key.Currency,
                    Status = UserMoneyTransferTaskStatus.Open,
                    UserId = group.Key.UserId,
                    UserAccount = group.Key.UserAccount,
                    Transfers = group.ToList()
                };

                using (var scope = new TransactionScope())
                {
                    _dalTask.Insert(taskData);
                    taskData.Transfers.ForEach(transfer => _dalTransfer.UpdateStatus(transfer, UserMoneyTransferStatus.Tasked));
                    scope.Complete();
                }

                new Tasks.UserMoneyTransferTask(_dependencyFactory, _engine, taskData).Enqueue();
            }
        }

        public void RegisterNewTransfer(UserMoneyTransfer transfer)
        {
            lock (_locker)
            {
                _openTransfers.Add(transfer);
                _whenStrategies.ForEach(strategy => strategy.RegisterNewEntry());
            }
        }

        public void RegisterPendingTransfers(List<UserMoneyTransfer> transfers)
        {
            lock (_locker)
            {
                _openTransfers.AddRange(transfers);
                transfers.ForEach(transfer => _whenStrategies.ForEach(strategy => strategy.RegisterNewEntry()));
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

            _whenStrategies?.ForEach(strategy => strategy.Dispose());
        }

    }
}
