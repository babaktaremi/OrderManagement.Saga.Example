namespace OrderManagement.Saga.Example.Database.EntityModels;

public class UserWalletEntity
{
    public Guid WalletId { get; set; }
    public Guid UserId { get; set; }
    public decimal WalletChargeAmount { get; set; }
    public UserEntity User { get; set; }
}