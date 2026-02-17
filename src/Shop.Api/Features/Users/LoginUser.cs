using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Abstractions.Authentication;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Features.Users;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    [Required]
    public required string Password { get; init; }
}

public sealed class LoginUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/users/login",
            async (
                [FromBody] LoginUserRequest request, 
                UserManager<ApplicationUser> userManager,
                IJwtProvider jwtProvider,
                HttpContext httpContext) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user is null ||
                    !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Results.Unauthorized();
                }

                var roles = await userManager.GetRolesAsync(user);

                string token = jwtProvider.GenerateToken(user, roles);

                httpContext.Response.Cookies.Append(JwtConstants.JwtCookieKey, token);

                return Results.Ok();
            })
            .WithTags(nameof(ApplicationUser));
    }
}
