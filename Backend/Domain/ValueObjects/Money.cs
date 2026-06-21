namespace Ecommerce.Domain.ValueObjects;

/// <summary>
/// Represents a positive monetary amount.
/// </summary>
public sealed class Money
{
    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The monetary amount cannot be negative.");
        }

        Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public decimal Amount { get; }

    public static Money Zero => new(0);

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);

    public static Money operator *(Money money, int quantity) => new(money.Amount * quantity);

    public override string ToString() => Amount.ToString("C");
}
