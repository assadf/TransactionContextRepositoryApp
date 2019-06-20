using System;
using System.Linq;
using System.Reflection;

namespace Version2
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string _assemblyWithRepositories;

        public RepositoryFactory(string assemblyWithRepositories)
        {
            _assemblyWithRepositories = assemblyWithRepositories;
        }

        public T Create<T>(IUnitOfWork unitOfWork)
        {
            if (!string.IsNullOrWhiteSpace(_assemblyWithRepositories))
            {
                Assembly.Load(_assemblyWithRepositories);
            }

            var type = typeof(T);
            var matchedType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && x.IsClass);

            if (matchedType == null)
            {
                throw new NotImplementedException("Repository Not Implemented");
            }

            return (T)Activator.CreateInstance(matchedType, unitOfWork);
        }
    }
}
