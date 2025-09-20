namespace OrderPoerations.Domain.Common;

public interface IAuditableEntity<T>
{
    public T CreatedBy { get; set; }
public DateTime CreatedAt { get; set; }
public T? UpdatedBy { get; set; }
public DateTime? UpdatedAt { get; set; }
public bool IsActive { get; set; }
}