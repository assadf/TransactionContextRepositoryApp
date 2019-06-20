using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Version2
{
    interface IUnitOfWorkFactory
    {
        Task CreateAsync(Func<IUnitOfWork, Task> normalFlow, Func<Task> exceptionFlow);
    }

    public class SqlUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public SqlUnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateAsync(Func<IUnitOfWork, Task> normalFlow, Func<Task> exceptionFlow)
        {
            using (var uow = new SqlUnitOfWork(new SqlConnection(_connectionString)))
            {
                try
                {
                    await uow.BeginAsync().ConfigureAwait(false);
                    await normalFlow.Invoke(uow).ConfigureAwait(false);
                    uow.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    uow.Rollback();
                    await exceptionFlow.Invoke().ConfigureAwait(false);
                }
            }
        }
    }
}
