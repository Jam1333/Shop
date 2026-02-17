using Shop.Api.Domain.Primitives;

namespace Shop.Api.Domain.Entities;

public class LineItem : Entity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
