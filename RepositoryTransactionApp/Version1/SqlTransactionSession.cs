namespace RepositoryTransactionApp.Version1
{
    public class SqlTransactionSession : ITransactionSession
    {
        public SqlTransactionSession()
        {
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            RepositoryContext.GetTransaction();
        }

        public void Commit()
        {
            RepositoryContext.GetTransaction().Commit();
        }

        public void Rollback()
        {
            RepositoryContext.GetTransaction().Rollback();
        }

        public void Dispose()
        {
            RepositoryContext.Dispose();
        }
    }
}
