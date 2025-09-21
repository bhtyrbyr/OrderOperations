namespace OrderOperations.Application.DTOs.BasketDTOs;

public class BasketViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<BasketItemViewModel> BasketItems { get; set; } = new();
}

public class BasketItemViewModel
{
    public int BasketItemId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; }
    public double TotalCost { get; set; }
}
