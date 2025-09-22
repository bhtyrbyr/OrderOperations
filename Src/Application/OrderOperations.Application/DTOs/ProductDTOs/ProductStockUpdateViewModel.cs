namespace OrderOperations.Application.DTOs.ProductDTOs;

public class ProductStockUpdateViewModel
{
    public Guid ProductId { get; set; }   // Stok eklenecek ürün ID'si
    public decimal Quantity { get; set; }  // Eklenecek miktar
}
