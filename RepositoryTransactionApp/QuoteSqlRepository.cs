using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using RepositoryTransactionApp.Version1;

namespace RepositoryTransactionApp
{
    public class QuoteSqlRepository : IQuoteRepository
    {
        public QuoteSqlRepository(string connectionString)
        {
            RepositoryContext.SetConnectionString(connectionString);
        }

        public async Task<int> SaveAsync(Quote quote)
        {
            var parametersList = new DynamicParameters(new { Product = quote.ProductName });

            var conn = RepositoryContext.GetConnection();

            Console.WriteLine($"ProductName={quote.ProductName}, Connection Id in SaveAsync Quote: {((SqlConnection) conn).ClientConnectionId}" );

            var id = await conn
                .QuerySingleAsync<int>(
                    "INSERT INTO dbo.Quote ( Product ) VALUES (@Product); SELECT CAST(SCOPE_IDENTITY() as int)", 
                    parametersList, 
                    RepositoryContext.GetTransaction())
                .ConfigureAwait(false);

            return id;
        }

        public async Task<int> SaveAsync(QuoteCustomer quoteCustomer)
        {
            var parametersList = new DynamicParameters(new
                {QuoteId = quoteCustomer.QuoteId, FullName = quoteCustomer.FullName});

            var conn = RepositoryContext.GetConnection();

            Console.WriteLine($"CustomerName={quoteCustomer.FullName}, Connection Id in SaveAsync QuoteCustomer: {((SqlConnection) conn).ClientConnectionId}");

            var id = await conn
                .QuerySingleAsync<int>(
                    "INSERT INTO dbo.QuoteCustomer ( QuoteId, FullName) VALUES ( @QuoteId, @FullName); SELECT CAST(SCOPE_IDENTITY() as int)",
                    parametersList,
                    RepositoryContext.GetTransaction())
                .ConfigureAwait(false);

            return id;
        }
    }
}
