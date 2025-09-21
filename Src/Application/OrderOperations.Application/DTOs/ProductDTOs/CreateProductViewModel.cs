namespace OrderOperations.Application.DTOs.ProductDTOs;

public class CreateProductViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public double Price { get; set; }
}
