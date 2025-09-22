using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Product : BaseAuditableEntity<Guid, Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public Stock Stock { get; set; }
    public byte[] RowVersion { get; set; } // Concurrency token
}
