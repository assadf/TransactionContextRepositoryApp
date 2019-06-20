using System;

namespace RepositoryTransactionApp.Version1
{
    public interface ITransactionSession : IDisposable
    {
        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
