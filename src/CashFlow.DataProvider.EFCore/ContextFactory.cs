using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CashFlow.DataProvider.EFCore
{
    public class ContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string _connection = "Host=localhost;Port=5432;Database=CashFlow;Username=postgres;Password=Ilybax1991odinokov!";
        public DataContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(_connection);
            return new DataContext(builder.Options);
        }
    }
}
