using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Domain.Constants;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Features.Products;

public class CreateProductRequest
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

public sealed class CreateProduct : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/products",
            [Authorize(Roles = Roles.Admin)]
            async ([FromBody] CreateProductRequest request, ApplicationDbContext dbContext) =>
            {
                var product = new Product
                {
                    Name = request.Name,
                    GroupNumber = request.GroupNumber,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    Cost = request.Cost,
                };

                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();

                return Results.Ok(product.Id);
            })
            .WithTags(nameof(Product));
    }
}
