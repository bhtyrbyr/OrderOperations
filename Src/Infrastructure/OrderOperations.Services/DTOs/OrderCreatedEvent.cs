namespace OrderOperations.Services.DTOs;

public class OrderCreatedEvent
{
    public Guid Id { get; set; }          
    public string Type { get; set; } = default!;
    public string Content { get; set; } = default!;
}

public class OrderCreatedContent
{
    public Guid OrderId { get; set; }
}