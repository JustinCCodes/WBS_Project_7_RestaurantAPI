namespace Restaurant.Api.Data;

// Database context
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}
