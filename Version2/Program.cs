using System;
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
            var repoFactory = new RepositoryFactory();
            services.AddSingleton<IRepositoryFactory>(repoFactory);

            try
            {
                MainAsync(repoFactory).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        public static async Task MainAsync(IRepositoryFactory repositoryFactory)
        {
            using (var uow = new SqlUnitOfWork(new SqlConnection("Server=SL-24BGV02;Database=Sandpit;Trusted_Connection=True;")))
            {
                try
                {
                    await uow.BeginAsync().ConfigureAwait(false);

                    var quoteRepository = repositoryFactory.Create<IQuoteRepository>(uow);
                    var quoteId = await quoteRepository.CreateAsync(new Quote("MyProduct")).ConfigureAwait(false);
                    await quoteRepository.CreateAsync(new QuoteCustomer(quoteId, "Joe Blog")).ConfigureAwait(false);

                    uow.Commit();
                }
                catch (Exception e)
                {
                    uow.Rollback();
                    throw;
                }
                
            }
        }
    }
}
