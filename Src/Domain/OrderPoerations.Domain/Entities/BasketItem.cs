using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class BasketItem : BaseAuditableEntity<int, Guid>
{
    public Product Product { get; set; }
    public double Amount { get; set; }
    public double TotalCost { get; set; }
}
