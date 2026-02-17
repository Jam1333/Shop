using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Products;

public sealed class GetProductById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/products/{id:guid}",
            async ([FromRoute] Guid id, ApplicationDbContext dbContext) =>
            {
                Product? product = await dbContext
                    .Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(product);
            })
            .WithTags(nameof(Product));
    }
}
