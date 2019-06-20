using System.Threading.Tasks;

namespace RepositoryTransactionApp
{
    public interface IQuoteRepository
    {
        Task<int> SaveAsync(Quote quote);

        Task<int> SaveAsync(QuoteCustomer quoteCustomer);

        
    }
}
