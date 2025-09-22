namespace OrderOperations.Application.DTOs.OrderDTOs;

public class CreateOrderFromBasketViewModel
{
    public Guid PersonId { get; set; }
    public Guid BasketId { get; set; }
}
