using Shop.Api.Domain.Primitives;

namespace Shop.Api.Domain.Entities;

public sealed class Review : Entity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string? Address { get; set; }
    public string Text { get; set; }
}
