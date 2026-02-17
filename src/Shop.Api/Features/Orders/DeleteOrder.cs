using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.Security.Claims;

namespace Shop.Api.Features.Orders;

public sealed class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/orders/{id:guid}",
            [Authorize]
            async ([FromRoute] Guid id, ApplicationDbContext dbContext, HttpContext httpContext) =>
            {
                string? currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId is null)
                {
                    return Results.Unauthorized();
                }

                Order? order = await dbContext
                    .Orders
                    .SingleOrDefaultAsync(o => o.Id == id);

                if (order is null)
                {
                    return Results.NotFound();
                }

                if (order.UserId != currentUserId)
                {
                    return Results.Forbid();
                }

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithTags(nameof(Order));
    }
}
