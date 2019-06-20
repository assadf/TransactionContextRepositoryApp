using System;

namespace RepositoryTransactionApp.Version1
{
    public interface ITransactionSessionFactory : IDisposable
    {
        ITransactionSession Create();
    }
}
