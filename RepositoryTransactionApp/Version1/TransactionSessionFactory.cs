using System;
using System.Threading;

namespace RepositoryTransactionApp.Version1
{
    public class TransactionSessionFactory : ITransactionSessionFactory
    {
        private readonly Type _transactionSessionType;

        private static readonly AsyncLocal<ITransactionSession> _transactionSession = new AsyncLocal<ITransactionSession>();

        public TransactionSessionFactory(Type transactionSessionType)
        {
            _transactionSessionType = transactionSessionType;
        }

        public ITransactionSession Create()
        {
            if (_transactionSessionType == typeof(SqlTransactionSession))
            {
                _transactionSession.Value = new SqlTransactionSession();
                return _transactionSession.Value;
            }

            throw new NotSupportedException("Transaction Session Type not supported");
        }

        public void Dispose()
        {
            _transactionSession.Value?.Dispose();
        }
    }
}
