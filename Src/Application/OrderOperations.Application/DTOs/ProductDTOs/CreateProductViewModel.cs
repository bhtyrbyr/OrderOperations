namespace OrderOperations.Application.DTOs.ProductDTOs;

public class CreateProductViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public double Price { get; set; }
}
