using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;

namespace BookLibrary.Storage.Repositories
{
    public abstract class Repository
    {
        public static string ConnectionString { get; set; } = string.Empty;
        protected QueryFactory DataBase { get; }
        protected Repository()
        {
            var connection = new SqlConnection(ConnectionString);
            var compiler = new SqlServerCompiler();
            DataBase = new QueryFactory(connection, compiler);
        }
    }
}
