namespace OrderOperations.Application.DTOs.ProductDTOs;

public class UpdateProductViewModel
{
    public Guid Id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public double Price { get; set; }
}
