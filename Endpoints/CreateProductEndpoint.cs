using Carter;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Endpoints;

public record CreateProductApiModel(string ProductName, decimal ProductPrice, int AvailableAmount);

public class CreateProductEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreateProduct", async (CreateProductApiModel apiModel, ShopDbContext db) =>
        {
            var product =
                ProductInventoryEntity.Create(apiModel.ProductName, apiModel.AvailableAmount, apiModel.ProductPrice);

            db.Products.Add(product);

            await db.SaveChangesAsync();

            return TypedResults.Created();
        }).WithName("CreateProduct")
        .WithOpenApi();
    }
}