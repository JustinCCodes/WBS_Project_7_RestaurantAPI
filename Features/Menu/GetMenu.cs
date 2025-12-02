namespace Restaurant.Api.Features.Menu;

public static class GetMenu
{
    // Handler to get menu items with optional filters
    public static async Task<IResult> HandleAsync(
        AppDbContext db, // Database context
        string? q, // Search query
        string? category, // Category filter
        decimal? minPrice, // Minimum price filter
        decimal? maxPrice) // Maximum price filter
    {
        // Base query
        var query = db.MenuItems.AsNoTracking().AsQueryable();

        // EF.Functions.Like for case insensitive search
        // cause .ToLower inside query causes client evaluation issues with SQLite
        if (!string.IsNullOrWhiteSpace(q))
        {
            var pattern = $"%{q}%"; // Wildcard pattern for LIKE
            query = query.Where(x =>
                EF.Functions.Like(x.Name, pattern) ||
                EF.Functions.Like(x.Description ?? string.Empty, pattern)
            );
        }

        // Filters by category
        if (!string.IsNullOrWhiteSpace(category))
        {
            // Exact match for category
            query = query.Where(x => x.Category == category);
        }

        // Filters by price range
        if (minPrice.HasValue)
        {
            // Minimum price filter
            query = query.Where(x => x.Price >= minPrice.Value);
        }

        // Filters by price range
        if (maxPrice.HasValue)
        {
            // Maximum price filter
            query = query.Where(x => x.Price <= maxPrice.Value);
        }

        // Executes query and get results
        var items = await query.ToListAsync();

        // Returns list of menu items
        return TypedResults.Ok(items);
    }
}
