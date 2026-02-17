using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Products;

public sealed class GetAllProducts : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/products",
            async ([FromQuery] int? groupNumber, [FromQuery] string? searchTerm, ApplicationDbContext dbContext) =>
            {
                IQueryable<Product> products = dbContext.Products.AsNoTracking();

                if (groupNumber is not null)
                {
                    products = products.Where(p => p.GroupNumber == groupNumber);
                }

                if (searchTerm is not null)
                {
                    products = products.Where(p => p.Name.Contains(searchTerm));
                }

                return Results.Ok(await products.ToListAsync());
            })
            .WithTags(nameof(Product));
    }
}
