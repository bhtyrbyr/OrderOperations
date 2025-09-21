namespace OrderOperations.Application.DTOs.ProductDTOs;

public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public double Price { get; set; }
}
