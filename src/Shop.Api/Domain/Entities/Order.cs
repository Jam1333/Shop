using Shop.Api.Domain.Primitives;

namespace Shop.Api.Domain.Entities;

public sealed class Order : Entity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string Region { get; set; }
    public string District { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string? Comment { get; set; }

    public List<LineItem> LineItems { get; set; } = null!;
}
