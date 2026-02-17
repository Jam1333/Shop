using Microsoft.AspNetCore.Identity;

namespace Shop.Api.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public List<Order> Orders { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}
