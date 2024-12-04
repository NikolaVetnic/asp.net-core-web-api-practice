namespace Ordering.Domain.ValueObjects;

public record Address
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected Address()
    {
    }

    /*
     * Setting these as nullable should be ok because it is enforced by
     * Of method. On the other hand, I need an empty ctor for the EF C-
     * ore, so it cannot be required. This should be addressed.
     */

    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string EmailAddress { get; init; }
    public required string AddressLine { get; init; }
    public required string Country { get; init; }
    public required string State { get; init; }
    public required string ZipCode { get; init; }

    public static Address Of(string firstName, string lastName, string emailAddress,
        string addressLine, string country, string state, string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            AddressLine = addressLine,
            Country = country,
            State = state,
            ZipCode = zipCode
        };
    }
}