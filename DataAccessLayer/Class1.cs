using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class TestConnection
    {
        private class TestClass
        {
            public int TestId { get; set; }
            public string Name { get; set; }
        }

        public void Test()
        {


        using (var scope = new TransactionScope())
            {

                // PostgeSQL-style connection string
                const string connstring = "Server=localhost;Port=5432;User Id=BisonUser;Password=sowa;Database=HedgerData;";
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                // quite complex sql statement
                string sql = "INSERT INTO TestTable(\"Name\") VALUES(@Name)";
                //var sql = "SELECT *  FROM TestTable";


                var command = conn.CreateCommand();
                command.CommandText = sql;
                command.Parameters.Add(new NpgsqlParameter("@Name2", "Direct"));
                command.Parameters.Add(new NpgsqlParameter("@TestId", NpgsqlDbType.Integer, 50, "TestId", ParameterDirection.Output, true, 0, 0, DataRowVersion.Current, 5));
                command.ExecuteNonQuery();

                conn.Close();

                scope.Complete();
            }
        }
    }
}
