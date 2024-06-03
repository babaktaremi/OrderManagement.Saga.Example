using System.ComponentModel.DataAnnotations;
using Carter;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;

namespace OrderManagement.Saga.Example.Endpoints;

public record ChargeUserWalletApiModel(Guid UserId, [Range(1, double.MaxValue)] decimal Amount);

public class ChargeUserWalletEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ChargeUserWallet", async (ChargeUserWalletApiModel apiModel, ShopDbContext db) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(c => c.UserId.Equals(apiModel.UserId));

            if (user is null)
                return Results.NotFound();

            user.UserWallet.WalletChargeAmount += apiModel.Amount;

            await db.SaveChangesAsync();

            return Results.Ok();
        }).WithName("ChargeUserWallet")
        .WithOpenApi();
    }
}