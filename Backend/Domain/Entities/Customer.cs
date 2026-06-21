namespace Ecommerce.Domain.Entities;

/// <summary>
/// Customer who purchases products in the store.
/// </summary>
public sealed class Customer
{
    public Customer(string fullName, string email, string address, string phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("The customer name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("Enter a valid email address.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("The delivery address is required.", nameof(address));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("The contact phone is required.", nameof(phone));
        }

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Address = address.Trim();
        Phone = phone.Trim();
    }

    public Guid Id { get; }
    public string FullName { get; }
    public string Email { get; }
    public string Address { get; }
    public string Phone { get; }
}
