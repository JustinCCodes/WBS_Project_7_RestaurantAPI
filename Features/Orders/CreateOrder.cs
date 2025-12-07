namespace Restaurant.Api.Features.Orders;

// Creating a new order
public static class CreateOrder
{
    // DTOs
    public record Request(List<OrderItemRequest> Items); // List of items in order
    public record OrderItemRequest(int MenuItemId, int Quantity); // MenuItem ID and quantity
    public record Response(int Id, decimal TotalAmount, DateTime OrderDate); // Order response details

    // Validator for Request
    public class Validator : AbstractValidator<Request>
    {
        // Constructor
        public Validator()
        {
            // Validates that there is at least one item and each item has valid MenuItemId and Quantity
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.");

            // Validates each item in the order
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                // MenuItemId must be greater than 0
                item.RuleFor(i => i.MenuItemId).GreaterThan(0);
                // Quantity must be between 1 and 99
                item.RuleFor(i => i.Quantity).GreaterThan(0).LessThan(100);
            });
        }
    }

    // Handler
    public static async Task<IResult> HandleAsync(
        AppDbContext db, // Database context
        IValidator<Request> validator, // Validator for request
        Request dto, // Incoming request data
        CancellationToken ct) // Cancellation token
    {
        // Validates incoming data
        var validationResult = await validator.ValidateAsync(dto, ct);
        // Checks if validation failed
        if (!validationResult.IsValid)
        {
            // Returns validation errors
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // Fetches all requested MenuItems from DB
        var itemIds = dto.Items.Select(i => i.MenuItemId).Distinct().ToList();

        var dbItems = await db.MenuItems
            .AsNoTracking() // Read only for price lookup
            .Where(x => itemIds.Contains(x.Id)) // Filter to requested items
            .ToDictionaryAsync(x => x.Id, ct); // Dictionary for performant lookup

        // Validates all items exist
        if (dbItems.Count != itemIds.Count)
        {
            // Some items were not found
            var missingIds = itemIds.Except(dbItems.Keys);
            // Returns 400 Bad Request with missing item IDs
            return TypedResults.BadRequest($"Menu items not found: {string.Join(", ", missingIds)}");
        }

        // Calculates total and prepare Order entity
        var newOrder = new Order
        {
            OrderDate = DateTime.Now, // Sets order date to now
            TotalAmount = 0 // Initial total amount
        };

        // Calculates total and prepare OrderItems
        foreach (var itemDto in dto.Items)
        {
            // Looks up MenuItem from fetched dictionary
            var menuItem = dbItems[itemDto.MenuItemId];
            // Calculates line total
            var lineTotal = menuItem.Price * itemDto.Quantity;

            // Adds OrderItem to Order
            newOrder.Items.Add(new OrderItem
            {
                // References MenuItem
                MenuItemId = menuItem.Id,
                // Sets quantity
                Quantity = itemDto.Quantity
            });

            // Accumulates total amount
            newOrder.TotalAmount += lineTotal;
        }

        // Saves Order to database
        db.Orders.Add(newOrder);
        // Marks Order as created
        await db.SaveChangesAsync(ct);

        // Prepares response DTO
        var response = new Response(newOrder.Id, newOrder.TotalAmount, newOrder.OrderDate);
        // Returns the created order details
        return TypedResults.Created($"/orders/{newOrder.Id}", response);
    }
}
