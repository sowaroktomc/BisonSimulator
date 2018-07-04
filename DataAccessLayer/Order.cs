using Dapper;
using Sowalabs.Bison.Data.Types;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class Order : DatabaseDal
    {
        private const string SqlInsert = @"
SELECT OrderInsert(@UserId, @Amount, @Status, @Entity, @OpenTimestamp, @Reference);
";

        public int Insert(Data.Order order)
        {
            using (var connection = GetNewConnection())
            {
                var parameters = new DynamicParameters(order);
                parameters.Output(order, x => x.OrderId);

                connection.Execute(SqlInsert, parameters);
                return order.OrderId;
            }
        }

        public List<Data.Order> GetOpenOrders()
        {
            using (var connection = GetNewConnection())
            {
                var sql = $"SELECT * FROM \"Order\" WHERE Status IN ('{OrderStatus.Open}', '{OrderStatus.Executing}')";
                return connection.Query<Data.Order>(sql).ToList();
            }
        }


    }
}
