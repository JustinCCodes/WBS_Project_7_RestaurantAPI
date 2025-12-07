namespace Restaurant.Api.Features.Reports;

// Generating daily report
public static class DailyReport
{
    // DTOs
    public record Response(DateOnly Date, int TotalOrders, decimal TotalRevenue, List<TopSellingItem> TopItems);
    public record TopSellingItem(string Name, int QuantitySold);

    // Handler for generating daily report
    public static async Task<IResult> HandleAsync(AppDbContext db, string date)
    {
        // Allowed date formats
        string[] formats = ["yyyy-MM-dd", "dd.MM.yyyy", "dd-MM-yyyy"];
        // Parses date with multiple formats
        if (!DateOnly.TryParseExact(date, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            // Returns BadRequest if date is invalid
            return TypedResults.BadRequest("Invalid date. Use 'dd.MM.yyyy' or ISO.");
        }

        // Defines start and end of the day
        var startOfDay = parsedDate.ToDateTime(TimeOnly.MinValue);
        var endOfDay = parsedDate.ToDateTime(TimeOnly.MaxValue);

        // Queries for order statistics
        var statsQuery = db.Orders
            .AsNoTracking() // Read only query
            .Where(x => x.OrderDate >= startOfDay && x.OrderDate <= endOfDay) // Filters by date
            .GroupBy(x => 1) // Groups all into single group
            .Select(g => new // Anonymous object for stats
            {
                Count = g.Count(), // Total orders
                Revenue = g.Sum(x => x.TotalAmount) // Total revenue
            })
            // Executes query and gets single result
            .FirstOrDefaultAsync();

        // Queries for top selling items
        var topItemsQuery = db.OrderItems
            .AsNoTracking() // Read only query
            .Where(oi => oi.Order!.OrderDate >= startOfDay && oi.Order.OrderDate <= endOfDay) // Filters by date
            .GroupBy(oi => new { oi.MenuItemId, oi.MenuItem!.Name }) // Groups by MenuItem
            .Select(g => new // Anonymous object for top selling items
            {
                Name = g.Key.Name, // MenuItem name
                QuantitySold = g.Sum(x => x.Quantity) // Total quantity sold
            })
            .OrderByDescending(x => x.QuantitySold) // Orders by quantity sold
            .Take(3) // Takes top 3 items
                     // Executes query and gets list
            .ToListAsync();

        // Awaits both queries in parallel
        await Task.WhenAll(statsQuery, topItemsQuery);

        // Retrieves results
        var orderStats = statsQuery.Result;
        var topItemsData = topItemsQuery.Result;

        // Maps top items to DTO
        var topItems = topItemsData
            .Select(x => new TopSellingItem(x.Name, x.QuantitySold)) // Maps to TopSellingItem DTO
            .ToList(); // Converts to list

        // Returns the daily report response
        return TypedResults.Ok(new Response(
            parsedDate, // Date
            orderStats?.Count ?? 0, // TotalOrders
            orderStats?.Revenue ?? 0m, // TotalRevenue
            topItems // TopItems
        ));
    }
}
