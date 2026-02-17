using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Shop.Api.Features.Orders;

public class CreateOrderRequest
{
    [Required]
    [MaxLength(256)]
    public required string Region { get; set; }
    [Required]
    [MaxLength(256)]
    public required string District { get; set; }
    [Required]
    [MaxLength(512)]
    public required string Address { get; set; }
    [Required]
    [MaxLength(12)]
    public required string PhoneNumber { get; set; }
    public string? Comment { get; private set; }
    [Required]
    public required Guid[] ProductIds { get; init; }
}

public sealed class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/orders",
            [Authorize]
            async ([FromBody] CreateOrderRequest request, ApplicationDbContext dbContext, HttpContext httpContext) =>
            {
                string? currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId is null)
                {
                    return Results.Unauthorized();
                }

                var order = new Order
                {
                    UserId = currentUserId,
                    Region = request.Region,
                    District = request.District,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Comment = request.Comment,
                };

                await dbContext.Orders.AddAsync(order);

                List<LineItem> products = await dbContext
                    .Products
                    .Where(p => request.ProductIds.Contains(p.Id))
                    .Select(p => new LineItem { ProductId = p.Id, OrderId = order.Id })
                    .ToListAsync();

                await dbContext.LineItems.AddRangeAsync(products);

                await dbContext.SaveChangesAsync();

                return Results.Ok(order.Id);
            })
            .WithTags(nameof(Order));
    }
}
