namespace Version2
{
    public interface IRepositoryFactory
    {
        T Create<T>(IUnitOfWork unitOfWork);
    }
}
