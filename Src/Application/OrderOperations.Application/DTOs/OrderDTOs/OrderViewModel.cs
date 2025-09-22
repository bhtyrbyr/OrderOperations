namespace OrderOperations.Application.DTOs.OrderDTOs;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BasketId { get; set; }
    public string OrderStatus { get; set; }
    public decimal TotalCost { get; set; }
    public string? Fail { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; } = new();
    public string? IdempotencyKey { get; set; }
}

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; }
    public double TotalCost { get; set; }
}

