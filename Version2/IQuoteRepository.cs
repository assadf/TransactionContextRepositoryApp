using System.Threading.Tasks;

namespace Version2
{
    public interface IQuoteRepository
    {
        Task<int> CreateAsync(Quote quote);

        Task<int> CreateAsync(QuoteCustomer quoteCustomer);
    }
}
