using OrderPoerations.Domain.Common;

namespace OrderPoerations.Domain.Entities;

public class OutboxMessage : BaseAuditableEntity<Guid, Guid>
{
    public string Type { get; set; } = default!;         
    public string Content { get; set; } = default!;      
    public int Attempts { get; set; }
    public string? Error { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
