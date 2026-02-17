using Shop.Api.Domain.Primitives;

namespace Shop.Api.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; set; }
    public int GroupNumber { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Cost { get; set; }

    public List<LineItem> LineItems { get; set; }
}
