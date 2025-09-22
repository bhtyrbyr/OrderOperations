namespace OrderOperations.Contracts;

public class OutboxMessageContract<T>
{
    public T Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
}
