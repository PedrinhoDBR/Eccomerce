namespace Ecommerce.Domain.Enums;

/// <summary>
/// Possible order states in the purchase flow.
/// </summary>
public enum OrderStatus
{
    Created = 1,
    Paid = 2,
    Shipped = 3,
    Delivered = 4,
    Canceled = 5
}
