using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Persistence.Context;

public class AppDbContext : IdentityDbContext<Person, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>(cfg =>
        {
            cfg.ToTable("Orders");
            cfg.Property(x => x.IdempotencyKey).HasMaxLength(100);
            cfg.HasIndex(x => new { x.PersonId, x.IdempotencyKey }).IsUnique()
                .HasFilter("\"IdempotencyKey\" IS NOT NULL");
        });

        builder.Entity<OutboxMessage>(cfg =>
        {
            cfg.ToTable("OutboxMessages");
            cfg.HasKey(x => x.Id);
            cfg.Property(x => x.Type).IsRequired().HasMaxLength(100);
            cfg.Property(x => x.Content).IsRequired();
            cfg.HasIndex(x => x.ProcessedAt); // dispatcher taramasını kolaylaştırır
        });
    }
}
