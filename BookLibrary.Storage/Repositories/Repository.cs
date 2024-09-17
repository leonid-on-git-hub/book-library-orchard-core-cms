using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;
using System.Data.SqlClient;

namespace BookLibrary.Storage.Repositories
{
    public abstract class Repository
    {
        public static string? SqlServerConnectionString { get; set; }
        public static string? SqLiteConnectionString { get; set; }

        protected QueryFactory DataBase { get; }
        protected Repository(IDbConnection connection)
        {
            Compiler compiler = connection switch
            {
                SqliteConnection => new SqliteCompiler(),
                SqlConnection => new SqlServerCompiler(),
                _ => throw new ArgumentException("Unsupported connection type")
            };
            connection.ConnectionString = connection switch
            {
                SqliteConnection => SqLiteConnectionString,
                SqlConnection => SqlServerConnectionString,
                _ => connection.ConnectionString
            };
            if (string.IsNullOrWhiteSpace(connection.ConnectionString))
            {
                throw new InvalidOperationException("Connection string is null or white space");
            }

            DataBase = new QueryFactory(connection, compiler);
        }
    }
}
