using MassTransit;

namespace OrderManagement.Saga.Example.Database.EntityModels;

public class UserEntity
{
    public Guid UserId { get; private set; }
    public string UserName { get;private set; }
    public UserWalletEntity UserWallet { get;private set; }
    public List<OrderEntity> UserOrders { get;private set; }

    private UserEntity()
    {
        
    }


    public static UserEntity Create(string userName)
    {
        var userId = Guid.NewGuid();

        var user = new UserEntity()
        {
            UserId = userId,
            UserName = userName,
            UserWallet = new UserWalletEntity()
            {
                UserId = userId,
                WalletId = Guid.NewGuid(),
                WalletChargeAmount = 0
            }
        };

        return user;
    }
}