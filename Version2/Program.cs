using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Version2
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Started");

            var services = new ServiceCollection();

            var uowFactory = new SqlUnitOfWorkFactory("Server=SL-24BGV02;Database=Sandpit;Trusted_Connection=True;");
            services.AddSingleton<IUnitOfWorkFactory>(uowFactory);

            var repoFactory = new RepositoryFactory("Version2");
            services.AddSingleton<IRepositoryFactory>(repoFactory);

            try
            {
                MainAsync(uowFactory, repoFactory).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        public static async Task MainAsync(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
        {
            var tasks = new List<Task>();

            for (var i = 0; i < 100; i++)
            {
                tasks.Add(CreateAsync(unitOfWorkFactory, repositoryFactory, $"Product {i}", $"Customer {i}"));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public static async Task CreateAsync(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory, string productName, string customerName)
        {
            await unitOfWorkFactory.ExecuteTransactionAsync(async (uow) =>
            {
                var quoteRepository = repositoryFactory.Create<IQuoteRepository>(uow);
                var quoteId = await quoteRepository.CreateAsync(new Quote(productName)).ConfigureAwait(false);
                await quoteRepository.CreateAsync(new QuoteCustomer(quoteId, customerName)).ConfigureAwait(false);
            }, async () =>
            {
                Console.WriteLine("Something went wrong!!!");
            });
        }
    }
}
