using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Orders;

public sealed class GetAllOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders",
            [Authorize]
            async ([FromQuery] string? userId, ApplicationDbContext dbContext) =>
            {
                IQueryable<Order> orders = dbContext
                    .Orders
                    .Include(o => o.User)
                    .Include(o => o.LineItems)
                    .ThenInclude(li => li.Product)
                    .AsNoTracking();

                if (userId is not null)
                {
                    orders = orders.Where(o => o.UserId == userId);
                }

                return Results.Ok(await orders.ToListAsync());
            })
            .WithTags(nameof(Order));
    }
}
