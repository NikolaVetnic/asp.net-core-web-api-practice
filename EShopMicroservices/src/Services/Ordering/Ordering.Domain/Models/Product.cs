namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }

    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        return product;
    }
}