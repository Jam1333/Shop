using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Reviews;

public sealed class GetReviewById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/reviews/{id:guid}",
            [Authorize]
            async (Guid id, ApplicationDbContext dbContext) =>
            {
                Review? review = await dbContext
                    .Reviews
                    .Include(r => r.User)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(r => r.Id == id);

                if (review is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(review);
            })
            .WithTags(nameof(Review)); ;
    }
}
