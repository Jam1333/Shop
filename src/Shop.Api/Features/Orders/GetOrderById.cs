using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Orders;

public sealed class GetOrderById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders/{id:guid}",
            [Authorize]
            async ([FromRoute] Guid id, ApplicationDbContext dbContext) =>
            {
                Order? order = await dbContext
                    .Orders
                    .Include(o => o.LineItems)
                    .ThenInclude(li => li.Product)
                    .SingleOrDefaultAsync(o => o.Id == id);

                if (order is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(order);
            })
            .WithTags(nameof(Order));
    }
}
