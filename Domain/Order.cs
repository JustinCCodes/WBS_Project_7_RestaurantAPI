namespace Restaurant.Api.Domain;

public class Order
{
    public int Id { get; set; } // Primary Key
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Date of order
    public decimal TotalAmount { get; set; } // Total amount of order

    // METHOD: Navigation Property - One Order has Many OrderItems
    public List<OrderItem> Items { get; set; } = []; // Collection of order items
}
