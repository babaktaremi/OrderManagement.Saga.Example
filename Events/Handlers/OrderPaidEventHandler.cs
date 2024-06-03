using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;
using OrderManagement.Saga.Example.Events.Models;
using OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

namespace OrderManagement.Saga.Example.Events.Handlers;

public class OrderPaidEventHandler(ShopDbContext db):IConsumer<OrderPaidEventModel>
{
    public async Task Consume(ConsumeContext<OrderPaidEventModel> context)
    {
        var order = await db.Orders.FirstOrDefaultAsync(c => c.OrderId.Equals(context.Message.OrderId));
        
        if(order is null)
            return;

        var userWallet = await db.UserWallet.FirstOrDefaultAsync(c => c.UserId.Equals(order.UserId));
        
        if(userWallet is null)
            return;

        userWallet.WalletChargeAmount -= order.Product.ProductPrice;
        
        order.UpdateOrderState(OrderEntity.OrderStates.Paid);

        await db.SaveChangesAsync();

        await context.Publish(new OrderPaidSagaEvent(order.OrderId, order.User.UserName));
    }
}