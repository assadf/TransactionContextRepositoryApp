using System;

namespace Version2
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public T Create<T>(IUnitOfWork unitOfWork)
        {
            if (typeof(T) == typeof(IQuoteRepository))
            {
                return (T)(object)new QuoteSqlRepository(unitOfWork);
            }

            throw new NotImplementedException("Repository Not Implemented");
        }
    }
}
