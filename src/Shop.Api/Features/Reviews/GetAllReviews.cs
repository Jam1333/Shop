using Carter;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Reviews;

public sealed class GetAllReviews : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/reviews", async (ApplicationDbContext dbContext) =>
        {
            List<Review> reviews = await dbContext
                .Reviews
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(reviews);
        })
        .WithTags(nameof(Review)); ;
    }
}
