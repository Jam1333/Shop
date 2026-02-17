using Carter;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Constants;

namespace Shop.Api.Features.Users;

public sealed class LogoutUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete(JwtConstants.JwtCookieKey);

            return Results.Ok();
        })
        .WithTags(nameof(ApplicationUser));
    }
}
