namespace Shop.Api.Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedOnUtc { get; protected set; } = DateTime.UtcNow;
}
