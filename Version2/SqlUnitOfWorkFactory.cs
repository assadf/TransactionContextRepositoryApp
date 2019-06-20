using System;
using System.Data.SqlClient;

namespace Version2
{
    interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }

    public class SqlUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public SqlUnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUnitOfWork Create()
        {
            return new SqlUnitOfWork(new SqlConnection(_connectionString));
        }
    }
}
