namespace Ordering.Domain.Models;

public class Customer : Entity<Guid>
{
    public required string Name { get; init; }
    public required string Email { get; init; }

    public static Customer Create(Guid id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };

        return customer;
    }
}