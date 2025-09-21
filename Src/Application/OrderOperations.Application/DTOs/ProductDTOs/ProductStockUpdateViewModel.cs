namespace OrderOperations.Application.DTOs.ProductDTOs;

public class ProductStockUpdateViewModel
{
    public Guid ProductId { get; set; }   // Stok eklenecek ürün ID'si
    public double Quantity { get; set; }  // Eklenecek miktar
}
