namespace OrderManagement.Saga.Example.Database.EntityModels;

public class OrderEntity
{
    public Guid OrderId { get;private set; }
    public Guid UserId { get;private set; }
    public Guid ProductId { get;private set; }
    public DateTime OrderCreateDate { get;private set; }
    public DateTime OrderUpdatedDate { get; private set; }
    public UserEntity User { get;private set; }
    public ProductInventoryEntity Product { get;private set; }
    public OrderStates OrderState { get; private set; }


    private OrderEntity()
    {
        
    }

    public static OrderEntity Create(Guid userId, Guid productId)
    {
        var order = new OrderEntity()
        {
            ProductId = productId,
            UserId = userId,
            OrderCreateDate = DateTime.Now,
            OrderId = Guid.NewGuid(),
            OrderState = OrderStates.Created
        };

        return order;
    }

    public void UpdateOrderState(OrderStates orderState)
    {
        this.OrderState = orderState;
       this.OrderUpdatedDate = DateTime.Now;
    }
    
    public enum OrderStates
    {
        Created,
        Paid,
        Processing,
        Completed
    }
}