namespace Restaurant.Api.Domain;

public class OrderItem
{
    public int Id { get; set; } // Primary Key
    public int OrderId { get; set; } // Foreign Key

    // METHOD: Navigation Property - Many OrderItems belong to One Order
    public Order? Order { get; set; } // Navigation to Order

    // METHOD: Navigation Property - Many OrderItems refer to One MenuItem
    public int MenuItemId { get; set; } // Foreign Key
    public MenuItem? MenuItem { get; set; } // Navigation to MenuItem

    public int Quantity { get; set; } // Quantity of menu items in the order
}
