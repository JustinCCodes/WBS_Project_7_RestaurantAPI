namespace Restaurant.Api.Domain;

public class MenuItem
{
    public int Id { get; set; } // Primary Key
    public required string Name { get; set; } // Name of menu item
    public string? Description { get; set; } // Description of menu item
    public decimal Price { get; set; } // Price of menu item
    public required string Category { get; set; } // Category of menu item

    // Allowed categories for menu items
    public static readonly string[] AllowedCategories =
    [
        "Burgers", "Salads", "Starters", "Sides", "Drinks", "Pizza", "Desserts"
    ];
}
