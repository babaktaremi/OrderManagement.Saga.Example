using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;
using OrderManagement.Saga.Example.Events.Models;
using OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

namespace OrderManagement.Saga.Example.Events.Handlers;

public class OrderCompletedEventHandler(ShopDbContext dbContext):IConsumer<OrderCompletedEventModel>
{
    public async Task Consume(ConsumeContext<OrderCompletedEventModel> context)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(c => c.OrderId.Equals(context.Message.OrderId));
        
        if(order is null)
            return;
        
        order.UpdateOrderState(OrderEntity.OrderStates.Completed);

        await dbContext.SaveChangesAsync();

        await context.Publish(new OrderCompletedSagaEvent(order.OrderId));
    }
}