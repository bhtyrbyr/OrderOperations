namespace OrderOperations.Contracts;

public class ConvertBasketToOrderContract
{
    public Guid BasketId { get; set; }
    public Guid ProcessorId { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    public DateTime OrderDate { get; set; }

}
