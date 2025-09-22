using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Stock : BaseAuditableEntity<long, Guid>
{
    public decimal Amount { get; set; }
    public byte[] RowVersion { get; set; } // Concurrency token
}
