using OrderPoerations.Domain.Common;
using OrderPoerations.Domain.Enums;

namespace OrderPoerations.Domain.Entities;

public class Order : BaseAuditableEntity<Guid, Guid>
{
    public Guid PersonId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public OrderStatusEnum Status { get; set; }
}
