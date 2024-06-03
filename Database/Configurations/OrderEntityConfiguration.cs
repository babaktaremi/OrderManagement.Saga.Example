using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Database.Configurations;

public class OrderEntityConfiguration:IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(c => c.OrderId);
        builder.HasOne(c => c.User).WithMany(c => c.UserOrders)
            .HasForeignKey(c => c.UserId);
        builder.HasOne(c => c.Product)
            .WithMany(c => c.RelatedOrders)
            .HasForeignKey(c => c.ProductId);

        builder.Property(c => c.OrderState)
            .HasConversion<EnumToStringConverter<OrderEntity.OrderStates>>().HasMaxLength(20);

        builder.Navigation(c => c.User).AutoInclude();
        builder.Navigation(c => c.Product).AutoInclude();

        builder.ToTable("Orders", "ord");
    }
}