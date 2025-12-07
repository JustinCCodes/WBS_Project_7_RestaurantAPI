namespace Restaurant.Api.Features.Orders;

// Getting list of orders
public static class GetOrders
{
    // Summary record for orders
    public record OrderSummary(int Id, DateTime Date, decimal Total, int ItemCount);
    // Detail record for orders
    public record OrderDetail(int Id, DateTime Date, decimal Total, List<OrderItemDetail> Items);
    // Detail record for order items
    public record OrderItemDetail(int MenuItemId, string Name, decimal UnitPrice, int Quantity, decimal LineTotal);

    // Handler to get list of orders
    public static async Task<IResult> GetListAsync(AppDbContext db)
    {
        // Fetches all orders with summary details
        var orders = await db.Orders
            // Read only query
            .AsNoTracking()
            // Orders sorted by most recent
            .OrderByDescending(x => x.OrderDate)
            // Projects to OrderSummary DTO
            .Select(x => new OrderSummary(
                x.Id, // Id
                x.OrderDate, // Date
                x.TotalAmount, // Total
                x.Items.Sum(i => i.Quantity) // ItemCount
                ))
            // Executes query and gets list
            .ToListAsync();

        // Returns list of order summaries
        return TypedResults.Ok(orders);
    }

    // Handler to get order by id
    public static async Task<IResult> GetByIdAsync(int id, AppDbContext db)
    {
        // Fetches order by id with detail information
        var order = await db.Orders
            // Read only query
            .AsNoTracking()
            // Uses split query to optimize loading related data
            .AsSplitQuery()
            // Filters by id
            .Where(x => x.Id == id)
            // Projects to OrderDetail DTO
            .Select(x => new OrderDetail(
                x.Id, // Id
                x.OrderDate, // Date
                x.TotalAmount, // Total
                x.Items.Select(i => new OrderItemDetail( // Maps order items
                    i.MenuItemId, // MenuItemId
                    i.MenuItem!.Name, // Name
                    i.MenuItem.Price, // UnitPrice
                    i.Quantity, // Quantity
                    i.MenuItem.Price * i.Quantity // LineTotal
                // Converts to list
                )).ToList()
            ))
            // Executes query and gets single order or null
            .FirstOrDefaultAsync();

        // Returns 404 if not found otherwise returns order details
        return order is null
            ? TypedResults.NotFound() // Returns 404 if not found
            : TypedResults.Ok(order); // Returns order details if found
    }
}
