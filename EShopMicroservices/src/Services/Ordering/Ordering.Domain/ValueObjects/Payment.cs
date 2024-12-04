namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public required string CardName;
    public required string CardNumber;
    public required string Cvv;
    public required string Expiration;
    public required int PaymentMethod;

    // ReSharper disable once MemberCanBePrivate.Global
    protected Payment()
    {
    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment
        {
            CardName = cardName,
            CardNumber = cardNumber,
            Cvv = cvv,
            Expiration = expiration,
            PaymentMethod = paymentMethod
        };
    }
}