using System;
using System.Collections.Generic;
using CashFlow.DataProvider.EFCore;
using CashFlow.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CashFlow.Server.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                appContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                //Log errors or do anything you think it's needed
                throw;
            }

            return host;
        }
        public static IHost InitializeDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                appContext.Incomes.AddRange(new []
                {
                    new Income { Name = "Зарплата", Price = 100000, OrderNumber = 1 },
                    new Income { Name = "Дивиденты по акциям компании Apple", Price = 250, OrderNumber = 2 }
                });

                appContext.Expenses.AddRange(new []
                {
                    new Expense { Name = "Квартира", Price = 15000, OrderNumber = 1 },
                    new Expense { Name = "Продукты", Price = 15000, OrderNumber = 2 },
                    new Expense { Name = "Прочие расходы", Price = 10000, OrderNumber = 3 },
                    new Expense { Name = "Платеж по ипотеке", Price = 30000, OrderNumber = 4 }
                });
                
                appContext.Assets.AddRange(new []
                {
                    new Asset{ Name = "Акции компании Apple", Quantity = 10, Price = 100000, OrderNumber = 1}
                });
               
                appContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //Log errors or do anything you think it's needed
                throw;
            }

            return host;
        }
    }
}
