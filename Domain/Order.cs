namespace Restaurant.Api.Domain;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    // METHOD: Navigation Property - One Order has Many OrderItems
    public List<OrderItem> Items { get; set; } = [];
}
