using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryTransactionApp.Version1;

namespace RepositoryTransactionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            var repo = new QuoteSqlRepository("Server=SL-24BGV02;Database=Sandpit;Trusted_Connection=True;");
            var sessionFactory = new TransactionSessionFactory(typeof(SqlTransactionSession));

            MainAsync(repo, sessionFactory).GetAwaiter().GetResult();

           Console.WriteLine("Finished");

           Console.ReadLine();
        }

        public static async Task MainAsync(IQuoteRepository repo, ITransactionSessionFactory transactionSessionFactory)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Insert(repo, transactionSessionFactory, $"Product {i}", $"Customer {i}"));
            }

            await Task.WhenAll(tasks);
        }

        public static async Task Insert(IQuoteRepository repo, ITransactionSessionFactory transactionSessionFactory, string productName, string customerName)
        {
            using (var trans = transactionSessionFactory.Create())
            {
                try
                {
                    var quoteId = await repo.SaveAsync(new Quote(productName));
                    await repo.SaveAsync(new QuoteCustomer(quoteId, customerName));
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
