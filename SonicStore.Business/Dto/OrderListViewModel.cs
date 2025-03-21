namespace SonicStore.Business.Dto;
public class OrderListViewModel
{
    public int Index { get; set; }
    public string CusName { get; set; }
    public string PaymentStatus { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
}
