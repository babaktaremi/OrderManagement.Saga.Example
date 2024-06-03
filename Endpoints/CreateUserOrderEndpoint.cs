using Carter;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Endpoints;

public record CreateUserOrderApiModel(Guid UserId, Guid ProductId);

public class CreateUserOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreateUserOrder", async (CreateUserOrderApiModel apiModel, ShopDbContext db) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(c => c.UserId.Equals(apiModel.UserId));

            if (user is null)
                return Results.NotFound(new
                {
                    ErrorMessage = "specified user not found"
                });

            var product = await db.Products.FirstOrDefaultAsync(c => c.ProductId.Equals(apiModel.ProductId));

            if (product is null)
                return Results.NotFound(new
                {
                    ErrorMessage = "specified product not found"
                });

            var order = OrderEntity.Create(user.UserId, product.ProductId);

            db.Orders.Add(order);

            await db.SaveChangesAsync();

            return Results.Created();
        }).WithName("CreateUserOrder")
            .WithOpenApi();
    }
}