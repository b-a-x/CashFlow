using CashFlow.DataProvider.EFCore.Configuration;
using CashFlow.DataProvider.EFCore.Model;
using CashFlow.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.DataProvider.EFCore
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new IncomeConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
        }
    }
}
