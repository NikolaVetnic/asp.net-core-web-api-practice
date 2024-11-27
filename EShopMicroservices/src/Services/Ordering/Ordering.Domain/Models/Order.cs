namespace Ordering.Domain.Models;

public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = [];

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public required Guid CustomerId { get; init; }
    public required string OrderName { get; init; }
    public required Address ShippingAddress { get; init; }
    public required Address BillingAddress { get; init; }
    public required Payment Payment { get; init; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
}