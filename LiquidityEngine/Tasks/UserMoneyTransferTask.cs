using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Data.Types;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using System;
using System.Transactions;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    public class UserMoneyTransferTask : BaseTask
    {

        private readonly Data.UserMoneyTransferTask _taskData;
        private readonly DataAccessLayer.UserMoneyTransferTask _dalTask = new DataAccessLayer.UserMoneyTransferTask();
        private readonly DataAccessLayer.UserMoneyTransfer _dalTransfer = new DataAccessLayer.UserMoneyTransfer();

        public UserMoneyTransferTask(IDependencyFactory dependencyFactory, LiquidityEngine engine, Data.UserMoneyTransferTask taskData) : base(dependencyFactory, engine)
        {
            _taskData = taskData;
        }

        public void Enqueue()
        {
            Engine.Queue.Enqueue(this);
        }

        protected override ExecutionStatus ExecuteTask()
        {
            switch (_taskData.Status)
            {
                case UserMoneyTransferTaskStatus.Open:
                    return SendToBank();
                case UserMoneyTransferTaskStatus.Sending:
                    //TODO Check Transfer status.
                    return ExecutionStatus.Error;
                case UserMoneyTransferTaskStatus.Sent:
                    return CheckTransactionStatus();
                default:
                    throw new ArgumentOutOfRangeException(nameof(_taskData.Status), _taskData.Status, "Unexpected task status.");
            }
        }

        private ExecutionStatus SendToBank()
        {
            Enum.TryParse<Currency>(_taskData.Currency, out var currency);

            var fromAccount = _taskData.Amount > 0 ? _taskData.UserAccount : EuwaxData.Solaris.GetAccount(currency).AccountNumber;
            var toAccount = _taskData.Amount < 0 ? _taskData.UserAccount : EuwaxData.Solaris.GetAccount(currency).AccountNumber;
            var bankApi = DependencyFactory.GetBankApi(EuwaxData.Solaris.SwiftCode);

            // First check account balance - if money is not on account, retry later.
            if (bankApi.GetAccountBalance(fromAccount) < Math.Abs(_taskData.Amount))
            {
                return ExecutionStatus.Repeat;
            }


            _dalTask.UpdateStatusToSending(_taskData, _taskData.TaskId.ToString());
            var bankTransactionId = bankApi.TransferMoney(fromAccount, toAccount, Math.Abs(_taskData.Amount), _taskData.Reference, string.Empty);
            _dalTask.UpdateStatusToSent(_taskData, bankTransactionId);

            return ExecuteTask();
        }

        private ExecutionStatus CheckTransactionStatus()
        {
            var bankApi = DependencyFactory.GetBankApi(EuwaxData.Solaris.SwiftCode);
            var statusResponse = bankApi.GetTransactionStatus(_taskData.BankTransactionId);

            switch (statusResponse.Status)
            {
                case BankTransactionStatusResponse.BankTransactionStatus.Pending:
                    return ExecutionStatus.Repeat;

                case BankTransactionStatusResponse.BankTransactionStatus.Executed:
                    using (var scope = new TransactionScope())
                    {
                        _dalTask.UpdateStatusToExecuted(_taskData);
                        _taskData.Transfers.ForEach(transfer => _dalTransfer.UpdateStatus(transfer, UserMoneyTransferStatus.Executed));

                        scope.Complete();
                    }

                    return ExecutionStatus.Done;

                case BankTransactionStatusResponse.BankTransactionStatus.Rejected:
                    _dalTask.UpdateStatusToError(_taskData);
                    return ExecutionStatus.Done;

                case BankTransactionStatusResponse.BankTransactionStatus.InsufficentFunds:
                    using (var scope = new TransactionScope())
                    {
                        _dalTask.UpdateStatusToError(_taskData);
                        _taskData.Transfers.ForEach(transfer => _dalTransfer.UpdateStatus(transfer, UserMoneyTransferStatus.New));

                        scope.Complete();
                    }
                    Engine.UserMoneyTransferGenerator.RegisterPendingTransfers(_taskData.Transfers);

                    return ExecutionStatus.Done;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
