namespace Restaurant.Api.Domain;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    // METHOD: Navigation Property - Many OrderItems belong to One Order
    public Order? Order { get; set; }

    // METHOD: Navigation Property - Many OrderItems refer to One MenuItem
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int Quantity { get; set; }
}
