using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace OrderPoerations.Domain.Common;

public class BaseAuditableEntity<T, Processor> : IEntity<T>, IAuditableEntity<Processor>
{
    [Key]
    public T? Id { get; set; }
    public Processor? CreatedBy { get; set; }
    public Processor? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
