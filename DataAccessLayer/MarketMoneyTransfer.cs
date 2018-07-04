using Dapper;
using Sowalabs.Bison.Data.Types;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class MarketMoneyTransfer : DatabaseDal
    {

        private const string SqlInsert = @"
SELECT MarketMoneyTransferInsert(@Market, @Status, @Amount, @Currency, @OpenTimeStamp, @Reference);
";

        public int Insert(Data.MarketMoneyTransfer transfer)
        {
            using (var connection = GetNewConnection())
            {
                var parameters = new DynamicParameters(transfer);
                parameters.Output(transfer, x => x.TransferId);

                connection.Execute(SqlInsert, parameters);
                return transfer.TransferId;
            }
        }

        public List<Data.MarketMoneyTransfer> GetOpenTransfers()
        {
            using (var connection = GetNewConnection())
            {
                var sql = $"SELECT * FROM MarketMoneyTransfer WHERE Status = '{MarketMoneyTransferStatus.Open}'";
                return connection.Query<Data.MarketMoneyTransfer>(sql).ToList();
            }
        }

    }
}
