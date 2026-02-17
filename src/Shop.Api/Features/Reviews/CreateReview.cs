using Carter;
using Microsoft.AspNetCore.Authorization;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Shop.Api.Features.Reviews;

public class CreateReviewRequest
{
    [Required]
    [MaxLength(256)]
    public required string Address { get; init; }
    [Required]
    [MaxLength(512)]
    public required string Text { get; init; }
}

public sealed class CreateReview : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/reviews",
            [Authorize]
            async (CreateReviewRequest request, ApplicationDbContext dbContext, HttpContext httpContext) =>
            {
                string? currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId is null)
                {
                    return Results.Unauthorized();
                }

                var review = new Review 
                {
                    UserId = currentUserId,
                    Address = request.Address,
                    Text = request.Text,
                };

                await dbContext.Reviews.AddAsync(review);
                await dbContext.SaveChangesAsync();

                return Results.Ok(review.Id);
            })
            .WithTags(nameof(Review));
    }
}
