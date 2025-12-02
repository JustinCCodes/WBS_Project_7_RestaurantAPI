namespace Restaurant.Api.Data;

// Database context
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>(); // DbSet for Orders
    public DbSet<MenuItem> MenuItems => Set<MenuItem>(); // DbSet for MenuItems
    public DbSet<OrderItem> OrderItems => Set<OrderItem>(); // DbSet for OrderItems
}
