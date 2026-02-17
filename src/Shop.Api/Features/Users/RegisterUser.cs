using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Features.Users;

public class RegisterUserRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(30)]
    public required string Name { get; init; }
    [Required]
    public required string Email { get; init; }
    [Required]
    [MinLength(8)]
    public required string Password { get; init; }
    public required string Role { get; init; }
}

public sealed class RegisterUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/users/register", 
            async (
                [FromBody] RegisterUserRequest request,
                ApplicationDbContext dbContext,
                UserManager<ApplicationUser> userManager) =>
            {
                if (await userManager.FindByEmailAsync(request.Email) is not null)
                {
                    return Results.BadRequest(new[] { new IdentityError { Description = "User with this email already exists" } });
                }

                var user = new ApplicationUser
                {
                    UserName = request.Name,
                    Email = request.Email,
                };

                using var transaction = dbContext.Database.BeginTransaction();

                var result = await userManager.CreateAsync(
                    user, 
                    request.Password);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }

                result = await userManager.AddToRoleAsync(user, request.Role);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }

                await transaction.CommitAsync();

                return Results.Ok(user.Id);
            })
            .WithTags(nameof(ApplicationUser)); ;
    }
}
