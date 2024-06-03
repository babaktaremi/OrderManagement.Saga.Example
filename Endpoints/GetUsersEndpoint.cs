using Carter;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;

namespace OrderManagement.Saga.Example.Endpoints;

public record UserWalletApiModel(Guid WalletId, decimal CurrentWalletAmount);

public record GetUsersApiModel(Guid UserId, string UserName, UserWalletApiModel UserWallet);

public class GetUsersEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Users", async (ShopDbContext db) =>
        {
            var users = await db.Users.AsNoTracking()
                .Select(c => new GetUsersApiModel(c.UserId, c.UserName,
                    new UserWalletApiModel(c.UserWallet.WalletId, c.UserWallet.WalletChargeAmount))).ToListAsync();


            return TypedResults.Ok(users);
        }).WithName("GetUsers")
        .WithOpenApi();
    }
}