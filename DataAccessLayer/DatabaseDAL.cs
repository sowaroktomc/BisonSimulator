using Npgsql;

namespace Sowalabs.Bison.DataAccessLayer
{
    public class DatabaseDal
    {

        protected NpgsqlConnection GetNewConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;User Id=BisonUser;Password=sowa;Database=HedgerData;");
        }
    }
}
