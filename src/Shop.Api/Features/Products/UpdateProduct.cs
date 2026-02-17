using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Constants;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Features.Products;

public class UpdateProductRequest
{
    [MinLength(3)]
    [MaxLength(256)]
    public required string Name { get; set; }
    [Required]
    public required int GroupNumber { get; set; }
    [Required]
    [MaxLength(1024)]
    public required string Description { get; set; }
    [Required]
    public required string ImageUrl { get; set; }
    [Required]
    public required decimal Cost { get; set; }
}

public sealed class UpdateProduct : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/products/{id:guid}",
            [Authorize(Roles = Roles.Admin)]
            async ([FromRoute] Guid id, [FromBody] UpdateProductRequest request, ApplicationDbContext dbContext) =>
            {
                Product? product = await dbContext.Products.SingleOrDefaultAsync(p => p.Id == id);

                if (product is null)
                {
                    return Results.NotFound();
                }

                product.Name = request.Name;
                product.GroupNumber = request.GroupNumber;
                product.Description = request.Description;
                product.ImageUrl = request.ImageUrl;
                product.Cost = request.Cost;

                await dbContext.SaveChangesAsync();

                return Results.Ok();
            })
            .WithTags(nameof(Product));
    }
}
