using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Stock : BaseAuditableEntity<long, Guid>
{
    public double Amount { get; set; }
}
