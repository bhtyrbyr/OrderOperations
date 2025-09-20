using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Category : BaseAuditableEntity<int, Guid>
{
    public string Name { get; set; }
}
