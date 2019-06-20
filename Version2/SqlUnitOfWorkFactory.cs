using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Version2
{
    interface IUnitOfWorkFactory
    {
        Task<IUnitOfWork> CreateAsync();
    }

    public class SqlUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public SqlUnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IUnitOfWork> CreateAsync()
        {
            var uow = new SqlUnitOfWork(new SqlConnection(_connectionString));
            await uow.BeginAsync().ConfigureAwait(false);
            return uow;
        }
    }
}
