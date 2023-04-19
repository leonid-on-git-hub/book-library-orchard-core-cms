using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;
using System.Data.SqlClient;

namespace BookLibrary.Storage.Repositories
{
    public abstract class Repository
    {
        public static string SqlServerConnectionString { get; set; } = string.Empty;
        protected QueryFactory DataBase { get; }
        protected Repository(IDbConnection connection)
        {
            if (connection is SqlConnection)
            {
                connection.ConnectionString = SqlServerConnectionString;
            }

            var compiler = new SqlServerCompiler();
            DataBase = new QueryFactory(connection, compiler);
        }
    }
}
