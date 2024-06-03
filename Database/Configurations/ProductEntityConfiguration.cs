using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Database.Configurations;

public class ProductEntityConfiguration:IEntityTypeConfiguration<ProductInventoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductInventoryEntity> builder)
    {
        builder.HasKey(c => c.ProductId);
        builder.Property(c => c.ProductName).HasMaxLength(100);
        builder.ToTable("Products", "prd");
    }
}