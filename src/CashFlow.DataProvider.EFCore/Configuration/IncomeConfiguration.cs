using CashFlow.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.DataProvider.EFCore.Configuration
{
    public class IncomeConfiguration : IEntityTypeConfiguration<Income>
    {
        public void Configure(EntityTypeBuilder<Income> builder)
        {
            builder.ToTable("Income");

            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Id).IsUnique();
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).HasColumnName("Name").IsRequired();
            builder.Property(u => u.OrderNumber).HasColumnName("OrderNumber").IsRequired();
            builder.Property(u => u.Price).HasColumnName("Price").IsRequired();
            builder.Property(u => u.UserId).HasColumnName("UserId").IsRequired();
            builder.HasIndex(p => p.UserId);
        }
    }
}
