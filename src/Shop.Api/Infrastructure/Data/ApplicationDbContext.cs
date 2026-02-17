using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Domain.Entities;

namespace Shop.Api.Infrastructure.Data;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<LineItem> LineItems { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasMany(p => p.LineItems)
            .WithOne(li => li.Product);

        builder.Entity<Order>()
            .HasMany(p => p.LineItems)
            .WithOne(li => li.Order);

        builder.Entity<ApplicationUser>()
            .HasMany(p => p.Orders)
            .WithOne(li => li.User);

        builder.Entity<ApplicationUser>()
            .HasMany(p => p.Reviews)
            .WithOne(li => li.User);
    }
}
