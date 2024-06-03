using Carter;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;

namespace OrderManagement.Saga.Example.Endpoints;

public record GetProductsApiModel(Guid ProductId, string ProductName, decimal ProductPrice, int AvailableAmount);

public class GetProductsEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Products", async (ShopDbContext db) =>
            {

                var products = await db.Products.AsNoTracking()
                    .Select(c =>
                        new GetProductsApiModel(c.ProductId, c.ProductName, c.ProductPrice, c.AvailableAmount))
                    .ToListAsync();

                return TypedResults.Ok(products);



            }).WithName("GetProducts")
            .WithOpenApi();
    }
}