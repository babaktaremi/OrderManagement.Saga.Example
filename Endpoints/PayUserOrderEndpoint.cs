using Carter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Events.Models;

namespace OrderManagement.Saga.Example.Endpoints;

public record PayUserOrderApiModel(Guid OrderId);

public class PayUserOrderEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/PayOrder", async (PayUserOrderApiModel apiModel, ShopDbContext db, IBus bus) =>
        {
            var order = await db.Orders.AsNoTracking().FirstOrDefaultAsync(c => c.OrderId.Equals(apiModel.OrderId));

            if (order is null)
                return Results.NotFound();

            await bus.Publish(new OrderPaidEventModel(order.OrderId, order.User.UserName));

            return Results.Accepted();
        }).WithName("PayOrder")
        .WithOpenApi();
    }
}