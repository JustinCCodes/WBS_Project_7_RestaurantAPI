namespace Restaurant.Api.Data.Seeding;

public static class DbSeeder
{
    // Seeds database with initial data if it is empty
    public static async Task SeedAsync(WebApplication app)
    {
        // Creates scope to get AppDbContext instance
        using var scope = app.Services.CreateScope(); // Creates a new scope for dependency injection
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Gets the AppDbContext from the service provider

        // Applies any pending migrations
        await context.Database.MigrateAsync();

        // Checks if database is already seeded
        if (await context.MenuItems.AnyAsync())
        {
            // Database already has data no need to seed
            return;
        }

        // Seeds database with initial data
        var items = new List<MenuItem>
        {
            new() { Name = "Cheeseburger", Price = 12.50m, Category = "Burgers", Description = "Klassischer Rindfleisch-Patty mit Cheddar." },
            new() { Name = "Veggie Burger", Price = 11.00m, Category = "Burgers", Description = "Kichererbsen-Patty mit Avocado-Creme." },
            new() { Name = "Chicken Wings (6stk)", Price = 8.99m, Category = "Starters", Description = "Scharf gewürzt mit BBQ-Dip." },
            new() { Name = "Caesar Salad", Price = 9.50m, Category = "Salads", Description = "Römersalat, Croutons, Parmesan." },
            new() { Name = "Pommes Frites", Price = 4.50m, Category = "Sides", Description = "Goldgelb und knusprig." },
            new() { Name = "Cola", Price = 3.00m, Category = "Drinks", Description = "0.33l eiskalt." },
            new() { Name = "Wasser", Price = 2.50m, Category = "Drinks", Description = "Still oder sprudelnd." },
            new() { Name = "Pizza Margherita", Price = 10.00m, Category = "Pizza", Description = "Tomatensauce, Mozzarella, Basilikum." },
            new() { Name = "Pizza Salami", Price = 12.00m, Category = "Pizza", Description = "Mit pikanter Salami." },
            new() { Name = "Tiramisu", Price = 6.50m, Category = "Desserts", Description = "Hausgemachtes italienisches Dessert." }
        };

        // Adds items to database
        context.MenuItems.AddRange(items);

        // Saves changes to database
        await context.SaveChangesAsync();
    }
}
