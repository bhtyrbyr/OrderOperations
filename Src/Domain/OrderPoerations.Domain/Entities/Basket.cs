using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class Basket : BaseAuditableEntity<Guid, Guid>
{
    public Guid UserId { get; set; }
    public ICollection<Product> BasketItems { get; set; }
}
