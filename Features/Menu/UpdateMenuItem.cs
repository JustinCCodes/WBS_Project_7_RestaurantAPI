namespace Restaurant.Api.Features.Menu;

public static class UpdateMenuItem
{
    // DTO for incoming request
    public record Request(string? Name, string? Description, decimal Price, string Category);

    // Validator for incoming request
    public class Validator : AbstractValidator<Request>
    {
        // Constructor to define validation rules
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 100); // Name is required and length between 3 and 100
            RuleFor(x => x.Price).GreaterThan(0); // Price must be greater than 0
            RuleFor(x => x.Category).NotEmpty().Must(c =>
                new[] { "Burgers", "Salads", "Starters", "Sides", "Drinks", "Pizza", "Desserts" }.Contains(c))
                .WithMessage("Invalid category."); // Category must be one of predefined values
        }
    }

    // Handler to update existing menu item
    public static async Task<IResult> HandleAsync(
        int id, // Menu item id
        AppDbContext db, // Database context
        IValidator<Request> validator, // Validator for request
        Request dto) // Incoming request data
    {
        // Validates the incoming data
        var validationResult = await validator.ValidateAsync(dto);
        // Checks if validation failed
        if (!validationResult.IsValid)
        {
            // Returns validation problem response if validation failed
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // Finds menu item by id
        var item = await db.MenuItems.FindAsync(id);
        // Checks if item exists
        if (item is null)
        {
            // Returns NotFound response if item does not exist
            return TypedResults.NotFound();
        }

        // Updates properties
        item.Name = dto.Name!; // Name is not null cause validation
        item.Description = dto.Description; // Description from request
        item.Price = dto.Price; // Price from request
        item.Category = dto.Category; // Category from request

        // Saves changes to database
        await db.SaveChangesAsync();

        // Returns NoContent response
        return TypedResults.NoContent();
    }
}
