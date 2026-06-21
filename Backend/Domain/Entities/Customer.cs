namespace Ecommerce.Domain.Entities;

/// <summary>
/// Customer who purchases products in the store.
/// </summary>
public sealed class Customer
{
    public Customer(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("The customer name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("Enter a valid email address.", nameof(email));
        }

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
    }

    public Guid Id { get; }
    public string FullName { get; }
    public string Email { get; }
}
