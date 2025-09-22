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
    public void MarkCreated() { Status = OrderStatusEnum.Created; /* Add DomainEvent */ }
    public void MarkProcessing() { Status = OrderStatusEnum.Processing; }
    public void MarkCompleted() { Status = OrderStatusEnum.Completed; }
    public void MarkFailed(string reason) { Status = OrderStatusEnum.Failed; Fail = reason; }
}
