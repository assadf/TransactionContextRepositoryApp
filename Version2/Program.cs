﻿using System;
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
            var repoFactory = new RepositoryFactory();
            var uowFactory = new SqlUnitOfWorkFactory("Server=SL-24BGV02;Database=Sandpit;Trusted_Connection=True;");
            services.AddSingleton<IRepositoryFactory>(repoFactory);
            services.AddSingleton<IUnitOfWorkFactory>(uowFactory);

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

            for (var i = 0; i < 1000; i++)
            {
                tasks.Add(CreateAsync(unitOfWorkFactory, repositoryFactory, $"Product {i}", $"Customer {i}"));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public static async Task CreateAsync(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory, string productName, string customerName)
        {
            using (var uow = unitOfWorkFactory.Create())
            {
                try
                {
                    await uow.BeginAsync().ConfigureAwait(false);

                    var quoteRepository = repositoryFactory.Create<IQuoteRepository>(uow);
                    var quoteId = await quoteRepository.CreateAsync(new Quote(productName)).ConfigureAwait(false);
                    await quoteRepository.CreateAsync(new QuoteCustomer(quoteId, customerName)).ConfigureAwait(false);

                    uow.Commit();
                }
                catch (Exception)
                {
                    uow.Rollback();
                    throw;
                }
            }
        }
    }
}
