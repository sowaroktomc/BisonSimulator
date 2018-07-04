using Dapper;
using Sowalabs.Bison.Data.Types;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class UserMoneyTransfer : DatabaseDal
    {

        private const string SqlInsert = @"
SELECT UserMoneyTransferInsert(@UserId, @UserAccount, @Status, @Amount, @Currency, @OpenTimeStamp, @Reference);
";

        private const string SqlUpdateStatus = @"
UPDATE UserMoneyTransfer
   SET Status = @Status
 WHERE TransferId = @TransferId
";

        public int Insert(Data.UserMoneyTransfer transfer)
        {
            using (var connection = GetNewConnection())
            {
                try
                {
                    var parameters = new DynamicParameters(transfer);
                    parameters.Output(transfer, x => x.TransferId);

                    connection.Execute(SqlInsert, parameters);
                    return transfer.TransferId;

                }
                catch (PostgresException ex)
                {
                    if (ex.SqlState != "23505") throw;

                    var sql = $"SELECT * FROM UserMoneyTransfer WHERE Reference = '{transfer.Reference}'";
                    var duplicate = connection.Query<Data.UserMoneyTransfer>(sql).Single();

                    if (transfer.UserId != duplicate.UserId || transfer.UserAccount != duplicate.UserAccount || transfer.Amount != duplicate.Amount || transfer.Currency != duplicate.Currency ||
                        transfer.Reference != duplicate.Reference) throw;

                    throw new DuplicateException(ex.Message, ex.InnerException);
                }
            }
        }

        public void UpdateStatus(Data.UserMoneyTransfer transfer, string newStatus)
        {
            transfer.Status = newStatus;
            using (var connection = GetNewConnection())
            {
                connection.Execute(SqlUpdateStatus, new {transfer.TransferId, Status = newStatus});
            }
        }

        public List<Data.UserMoneyTransfer> GetNewTransfers()
        {
            using (var connection = GetNewConnection())
            {
                var sql = $"SELECT * FROM UserMoneyTransfer WHERE Status = '{UserMoneyTransferStatus.New}'";
                return connection.Query<Data.UserMoneyTransfer>(sql).ToList();
            }
        }

    }
}
