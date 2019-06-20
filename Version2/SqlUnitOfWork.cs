using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Version2
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        Task BeginAsync();

        void Commit();

        void Rollback();
    }


    public class SqlUnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;

        public SqlUnitOfWork(SqlConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        public async Task BeginAsync()
        {
            var conn = (SqlConnection) Connection;
            await conn.OpenAsync().ConfigureAwait(false);
            Transaction = conn.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Dispose()
        {
            Connection?.Dispose();
            Transaction?.Dispose();
        }
    }
}
