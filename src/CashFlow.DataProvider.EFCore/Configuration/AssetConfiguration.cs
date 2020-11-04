using CashFlow.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.DataProvider.EFCore.Configuration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Id).IsUnique();
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).HasColumnName("Name").IsRequired();
            builder.Property(u => u.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(u => u.OrderNumber).HasColumnName("OrderNumber").IsRequired();
            builder.Property(u => u.Price).HasColumnName("Price").IsRequired();
            builder.Property(u => u.UserId).HasColumnName("UserId").IsRequired();
            builder.HasIndex(p => p.UserId);
        }
    }
}
