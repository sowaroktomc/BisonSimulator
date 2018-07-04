using Dapper;
using Sowalabs.Bison.Data.Types;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class UserMoneyTransferTask : DatabaseDal
    {

        private const string SqlInsertTask = @"
SELECT UserMoneyTransferTaskInsert(@UserId, @UserAccount, @Status, @Amount, @Currency, @Reference);
";

        private const string SqlUpdateStatus = @"
UPDATE UserMoneyTransferTask
   SET Status = @Status
 WHERE TaskId = @TaskId
";
        private const string SqlUpdateStatusAndReference = @"
UPDATE UserMoneyTransferTask
   SET Status = @Status,
       Reference = @Reference
 WHERE TaskId = @TaskId
";

        private const string SqlUpdateStatusToExecuted = @"
UPDATE UserMoneyTransferTask
   SET Status = @Status,
       Executed = now()
 WHERE TaskId = @TaskId
";

        private const string SqlUpdateStatusAndBankTransId = @"
UPDATE UserMoneyTransferTask
   SET Status = @Status,
       BankTransactionId = @BankTransactionId
 WHERE TaskId = @TaskId
";

        private const string SqlInsertTT = @"
SELECT UserMoneyTransferTTInsert(@TaskId, @TransferId);
";

        private const string SqlSelectTaskedTransfers = @"
SELECT UMT.*
  FROM UserMoneyTransfer UMT
  JOIN UserMoneyTransferTT UMTT ON UMT.TransferID = UMTT.TransferID
 WHERE UMTT.TaskID = @TaskID
";


        public int Insert(Data.UserMoneyTransferTask task)
        {
            using (var connection = GetNewConnection())
            {
                var parameters = new DynamicParameters(task);
                parameters.Output(task, x => x.TaskId);

                connection.Execute(SqlInsertTask, parameters);

                task.Transfers.ForEach(transfer => connection.Execute(SqlInsertTT, new {task.TaskId, transfer.TransferId}));

                return task.TaskId;
            }
        }

        public void UpdateStatusToSending(Data.UserMoneyTransferTask task, string reference)
        {
            task.Status = UserMoneyTransferTaskStatus.Sending;
            task.Reference = reference;

            using (var connection = GetNewConnection())
            {
                connection.Execute(SqlUpdateStatusAndReference, new { task.TaskId, task.Status, task.Reference });
            }
        }

        public void UpdateStatusToSent(Data.UserMoneyTransferTask task, string bankTransactionId)
        {
            task.Status = UserMoneyTransferTaskStatus.Sent;
            task.BankTransactionId = bankTransactionId;

            using (var connection = GetNewConnection())
            {
                connection.Execute(SqlUpdateStatusAndBankTransId, new { task.TaskId, task.Status, task.BankTransactionId });
            }
        }

        public void UpdateStatusToExecuted(Data.UserMoneyTransferTask task)
        {
            task.Status = UserMoneyTransferTaskStatus.Executed;

            using (var connection = GetNewConnection())
            {
                connection.Execute(SqlUpdateStatusToExecuted, new { task.TaskId, task.Status });
            }
        }

        public void UpdateStatusToError(Data.UserMoneyTransferTask task)
        {
            task.Status = UserMoneyTransferTaskStatus.Error;

            using (var connection = GetNewConnection())
            {
                connection.Execute(SqlUpdateStatus, new { task.TaskId, task.Status });
            }
        }
        public List<Data.UserMoneyTransferTask> GetOpenTasks()
        {
            using (var connection = GetNewConnection())
            {
                var sql = $"SELECT * FROM UserMoneyTransferTask WHERE Status <> '{UserMoneyTransferTaskStatus.Executed}'";
                var tasks = connection.Query<Data.UserMoneyTransferTask>(sql).ToList();

                tasks.ForEach(task => task.Transfers.AddRange(connection.Query<Data.UserMoneyTransfer>(SqlSelectTaskedTransfers, task.TaskId)));

                return tasks;
            }
        }

    }
}
