using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace RepositoryTransactionApp.Version1
{
    public static class RepositoryContext
    {
        private static string _connectionString;

        private static readonly AsyncLocal<IDbConnection> _connection = new AsyncLocal<IDbConnection>();

        private static readonly AsyncLocal<IDbTransaction> _transaction = new AsyncLocal<IDbTransaction>();

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static IDbConnection GetConnection()
        {
            return _connection.Value ?? (_connection.Value = new SqlConnection(_connectionString));
        }

        public static IDbTransaction GetTransaction()
        {

            if (_transaction.Value == null)
            {
                var conn = GetConnection();
                conn.Open();

                _transaction.Value = conn.BeginTransaction();
            }

            return _transaction.Value;
        }

        public static void Dispose()
        {
            if (_transaction.Value != null)
            {
                _transaction.Value.Dispose();
            }

            _connection.Value.Dispose();

            _transaction.Value = null;
            _connection.Value = null;
        }
    }
}
