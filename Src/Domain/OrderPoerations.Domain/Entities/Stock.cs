using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Stock : BaseAuditableEntity<long, Guid>
{
    public Product Product { get; set; }
    public double Amount { get; set; }
}
