using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Version2
{
    public class QuoteSqlRepository : IQuoteRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuoteSqlRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<int> CreateAsync(Quote quote)
        {
            var parametersList = new DynamicParameters(new { Product = quote.ProductName });

            Console.WriteLine($"ProductName={quote.ProductName}, Connection Id in SaveAsync Quote: {((SqlConnection)_unitOfWork.Connection).ClientConnectionId}");

            var id = await _unitOfWork.Connection
                .QuerySingleAsync<int>(
                    "INSERT INTO dbo.Quote ( Product ) VALUES (@Product); SELECT CAST(SCOPE_IDENTITY() as int)",
                    parametersList,
                    _unitOfWork.Transaction)
                .ConfigureAwait(false);

            return id;
        }

        public async Task<int> CreateAsync(QuoteCustomer quoteCustomer)
        {
            var parametersList = new DynamicParameters(new { QuoteId = quoteCustomer.QuoteId, FullName = quoteCustomer.FullName });

            Console.WriteLine($"CustomerName={quoteCustomer.FullName}, Connection Id in SaveAsync QuoteCustomer: {((SqlConnection)_unitOfWork.Connection).ClientConnectionId}");

            var id = await _unitOfWork.Connection
                .QuerySingleAsync<int>(
                    "INSERT INTO dbo.QuoteCustomer ( QuoteId, FullName) VALUES ( @QuoteId, @FullName); SELECT CAST(SCOPE_IDENTITY() as int)",
                    parametersList,
                    _unitOfWork.Transaction)
                .ConfigureAwait(false);

            return id;
        }
    }
}
