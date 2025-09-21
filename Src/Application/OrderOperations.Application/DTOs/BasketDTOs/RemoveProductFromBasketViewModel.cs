namespace OrderOperations.Application.DTOs.BasketDTOs;

public class RemoveProductFromBasketViewModel
{
    public Guid BasketId { get; set; }
    public int BasketItemId { get; set; } // Sepetten çıkarılacak item
}
