using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;
using OrderManagement.Saga.Example.Events.Models;
using OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

namespace OrderManagement.Saga.Example.Events.Handlers;

public class OrderProductInventoryProcessedHandler(ShopDbContext db):IConsumer<OrderProductInventoryProcessedEventModel>
{
    public async Task Consume(ConsumeContext<OrderProductInventoryProcessedEventModel> context)
    {
        var order = await db.Orders.FirstOrDefaultAsync(c => c.OrderId.Equals(context.Message.OrderId));
        if(order is null)
            return;
        
        var product = await db.Products.FirstOrDefaultAsync(c => c.ProductId.Equals(order.ProductId));
        
        if(product is null)
            return;

        product.DecreaseAvailableProducts();
        
        order.UpdateOrderState(OrderEntity.OrderStates.Processing);

        await db.SaveChangesAsync();

        await context.Publish(new OrderInventoryProcessedSagaEvent(order.OrderId));
    }
}