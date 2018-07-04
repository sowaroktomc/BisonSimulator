using Dapper;
using Sowalabs.Bison.Data.Types;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class Withdrawal : DatabaseDal
    {

        private const string SqlInsert = @"
SELECT WithdrawalInsert(UserId, Status, ToName, ToIban, ToBic, ToAddress, ToPostalCode, ToCity, ToCountryCode, Amount, Currency, OpenTimeStamp, InReference, OutReference);
";

        public int Insert(Data.Withdrawal withdrawal)
        {
            using (var connection = GetNewConnection())
            {
                var parameters = new DynamicParameters(withdrawal);
                parameters.Output(withdrawal, x => x.WithdrawalId);

                connection.Execute(SqlInsert, parameters);
                return withdrawal.WithdrawalId;
            }
        }

        public List<Data.Withdrawal> GetOpenWithdrawals()
        {
            using (var connection = GetNewConnection())
            {
                var sql = $"SELECT * FROM Withdrawal WHERE Status = '{WithdrawalStatus.Open}'";
                return connection.Query<Data.Withdrawal>(sql).ToList();
            }
        }

    }
}
