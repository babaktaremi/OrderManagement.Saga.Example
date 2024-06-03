namespace OrderManagement.Saga.Example.Database.EntityModels;

public class ProductInventoryEntity
{
    public Guid ProductId { get;private set; }
    public string ProductName { get;private set; }
    public decimal ProductPrice { get;private set; }
    public int AvailableAmount { get;private set; }
    public List<OrderEntity> RelatedOrders { get;private set; }

    private ProductInventoryEntity()
    {
        
    }

    public static ProductInventoryEntity Create(string productName, int availableAmount, decimal productPrice)
    {
        var product = new ProductInventoryEntity()
        {
            AvailableAmount = availableAmount,
            ProductId = Guid.NewGuid(),
            ProductName = productName,
            ProductPrice = productPrice,
        };

        return product;
    }

    public void DecreaseAvailableProducts()
    {
        this.AvailableAmount--;
    }
}