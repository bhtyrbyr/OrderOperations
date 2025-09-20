namespace OrderPoerations.Domain.Common;

public class BaseEntity<T> : IEntity<T>
{
    public required T Id { get; set; }
}