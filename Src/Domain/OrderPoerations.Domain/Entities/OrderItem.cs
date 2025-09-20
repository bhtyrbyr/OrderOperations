using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class OrderItem : BaseAuditableEntity<int, Guid>
{
    public Guid OrderId { get; set; }
    public Product Product { get; set; }
    public double Amount { get; set; }
    public double TotalCost { get; set; }
}
