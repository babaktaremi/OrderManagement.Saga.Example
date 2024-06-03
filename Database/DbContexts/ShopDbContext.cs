using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.EntityModels;
using OrderManagement.Saga.Example.Sagas.OrderSaga;

namespace OrderManagement.Saga.Example.Database.DbContexts;

public class ShopDbContext:DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options):base(options)
    {
        
    }

    public DbSet<UserEntity> Users => base.Set<UserEntity>();
    public DbSet<ProductInventoryEntity> Products => base.Set<ProductInventoryEntity>();
    public DbSet<UserWalletEntity> UserWallet => base.Set<UserWalletEntity>();
    public DbSet<OrderEntity> Orders => base.Set<OrderEntity>();
    public DbSet<OrderSagaStateMachineInstance> OrderSaga => base.Set<OrderSagaStateMachineInstance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopDbContext).Assembly);

        modelBuilder.Entity<OrderSagaStateMachineInstance>().HasKey(c => c.CorrelationId);
        base.OnModelCreating(modelBuilder);
    }
}