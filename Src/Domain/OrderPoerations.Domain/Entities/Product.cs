using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Product : BaseAuditableEntity<Guid, Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public double Price { get; set; }
}
