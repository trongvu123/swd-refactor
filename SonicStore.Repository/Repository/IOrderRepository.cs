namespace SonicStore.Repository.Repository;

public interface IOrderRepository
{
    Task<List<OrderListDto>> GetOrderListAsync();
}
public class OrderListDto
{
    public int Index { get; set; }
    public string CusName { get; set; }
    public string PaymentStatus { get; set; }
    public string OrderStatus { get; set; }
    public DateTime? OrderDate { get; set; } // Thay đổi thành DateTime?
}