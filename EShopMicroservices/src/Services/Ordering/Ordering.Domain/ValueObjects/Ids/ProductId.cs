namespace Ordering.Domain.ValueObjects.Ids;

public record ProductId
{
    private ProductId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty) throw new DomainException("ProductId cannot be empty.");

        return new ProductId(value);
    }
}