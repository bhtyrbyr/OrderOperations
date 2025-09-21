namespace OrderOperations.Application.DTOs.BasketDTOs;

public class AddProductToBasketViewModel
{
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
    public double Quantity { get; set; } // Eklenecek ürün adedi
}
