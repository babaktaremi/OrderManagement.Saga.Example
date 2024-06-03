using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Database.Configurations;

public class UserEntityConfiguration:IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(c => c.UserId);
        builder.Property(c => c.UserName).HasMaxLength(200);
        builder.HasOne(c => c.UserWallet).WithOne(c => c.User)
            .HasForeignKey<UserWalletEntity>(c => c.UserId);

        builder.Navigation(c => c.UserWallet).AutoInclude();

        builder.ToTable("users", "usr");
    }
}