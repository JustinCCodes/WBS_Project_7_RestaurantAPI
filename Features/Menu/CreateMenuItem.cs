namespace Restaurant.Api.Features.Menu;

public static class CreateMenuItem
{
    // DTO for creating menu item
    public record Request(string Name, string? Description, decimal Price, string Category);

    // Validator for incoming request
    public class Validator : AbstractValidator<Request>
    {
        // Constructor to define validation rules
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100); // Name is required and max length 100
            RuleFor(x => x.Price).GreaterThan(0); // Price must be greater than 0
            RuleFor(x => x.Category).NotEmpty().Must(c =>
                new[] { "Burgers", "Salads", "Starters", "Sides", "Drinks", "Pizza", "Desserts" }.Contains(c))
                .WithMessage("Invalid category."); // Category must be one of predefined values
        }
    }

    // Handler to create new menu item
    public static async Task<IResult> HandleAsync(
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
        // Creates new menu item
        var item = new MenuItem
        {
            Name = dto.Name, // Name from request
            Description = dto.Description, // Description from request
            Price = dto.Price, // Price from request
            Category = dto.Category // Category from request
        };

        // Adds item to database
        db.MenuItems.Add(item);

        // Saves changes to database
        await db.SaveChangesAsync();

        // Returns Created response with the new item
        return TypedResults.Created($"/menu/{item.Id}", item);
    }
}
