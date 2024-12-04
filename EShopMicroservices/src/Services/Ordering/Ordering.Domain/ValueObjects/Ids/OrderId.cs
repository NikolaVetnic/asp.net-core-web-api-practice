namespace Ordering.Domain.ValueObjects.Ids;

public record OrderId
{
    private OrderId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty) throw new DomainException("OrderId cannot be empty.");

        return new OrderId(value);
    }
}