using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Constants;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;

namespace Shop.Api.Features.Reviews;

public sealed class DeleteReview : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/reviews/{id:guid}",
            [Authorize(Roles = Roles.Admin)]
            async ([FromRoute] Guid id, ApplicationDbContext dbContext) =>
            {
                Review? review = await dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);

                if (review is null)
                {
                    return Results.NotFound();
                }

                dbContext.Reviews.Remove(review);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithTags(nameof(Review)); ;
    }
}
