using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Constants;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Products;

public sealed class DeleteProduct : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/products/{id:guid}",
            [Authorize(Roles = Roles.Admin)]
            async ([FromRoute] Guid id, ApplicationDbContext dbContext) =>
            {
                Product? product = await dbContext
                    .Products
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product is null)
                {
                    return Results.NotFound();
                }

                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithTags(nameof(Product));
    }
}
