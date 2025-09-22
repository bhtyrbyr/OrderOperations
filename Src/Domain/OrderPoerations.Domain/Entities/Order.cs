using OrderPoerations.Domain.Common;
using OrderPoerations.Domain.Enums;

namespace OrderPoerations.Domain.Entities;

public class Order : BaseAuditableEntity<Guid, Guid>
{
    public int OrderNumber { get; set; }
    public Guid PersonId { get; set; }
    public OrderStatusEnum Status { get; set; }
    public decimal TotalCost { get; set; }
    public string Fail { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
