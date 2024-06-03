using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Database.Configurations;

public class WalletEntityConfiguration:IEntityTypeConfiguration<UserWalletEntity>
{
    public void Configure(EntityTypeBuilder<UserWalletEntity> builder)
    {
        builder.HasKey(c => c.WalletId);
        builder.Property(c => c.WalletChargeAmount)
            .HasPrecision(9, 3);

        builder.Navigation(c => c.User).AutoInclude();
        
        builder.ToTable("UserWallets", "usr");
    }
}