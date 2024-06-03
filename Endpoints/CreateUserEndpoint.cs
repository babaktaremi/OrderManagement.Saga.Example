using Carter;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Database.EntityModels;

namespace OrderManagement.Saga.Example.Endpoints;

public record CreateUserApiModel(string UserName);

public class CreateUserEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/CreateUser", async (CreateUserApiModel apiModel, ShopDbContext db) =>
        {
            var user = UserEntity.Create(apiModel.UserName);

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return TypedResults.Created();
        }).WithName("CreateUser")
        .WithOpenApi();
    }
}