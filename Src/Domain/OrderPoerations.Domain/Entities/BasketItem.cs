using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class BasketItem : BaseAuditableEntity<int, Guid>
{
    public Product Product { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalCost { get; set; }
}
