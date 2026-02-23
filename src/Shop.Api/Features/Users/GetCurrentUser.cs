using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Shop.Api.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shop.Api.Features.Users;

public sealed class GetCurrentUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/users/current", 
            [Authorize] async (UserManager<ApplicationUser> userManager, HttpContext httpContext) =>
            {
                string? currentUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId is null)
                {
                    return Results.Unauthorized();
                }

                var currentUser = await userManager.FindByIdAsync(currentUserId);

                if (currentUser is null)
                {
                    return Results.Unauthorized();
                }

                var roles = userManager.GetRolesAsync(currentUser);

                return Results.Ok(new { info = currentUser, roles });
            })
            .WithTags(nameof(ApplicationUser));
    }
}
