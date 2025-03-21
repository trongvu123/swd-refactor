using SonicStore.Repository.Entity;
using Microsoft.EntityFrameworkCore;
namespace SonicStore.Repository.Repository;
public class OrderRepository : IOrderRepository
{
    private readonly SonicStoreContext _context;

    public OrderRepository(SonicStoreContext context)
    {
        _context = context;
    }

    public async Task<List<OrderListDto>> GetOrderListAsync()
    {
        var data = await (from checkout in _context.Orders
                          join cart in _context.OrderDetails
                              on checkout.CartId equals cart.Id
                          join payment in _context.Payments
                              on checkout.PaymentId equals payment.Id
                          join orderStatus in _context.StatusOrders
                              on checkout.Id equals orderStatus.OrderId
                          join paymentStatus in _context.StatusPayments
                              on payment.Id equals paymentStatus.Payment_id
                          join customer in _context.Users
                              on cart.CustomerId equals customer.Id
                          group new
                          {
                              CusName = customer.FullName,
                              PaymentStatus = paymentStatus.Type,
                              OrderStatus = orderStatus.Type,
                              OrderDate = checkout.OrderDate
                          } by new { checkout.index } into groupedData
                          select new OrderListDto
                          {
                              Index = groupedData.Key.index,
                              CusName = groupedData.First().CusName,
                              PaymentStatus = groupedData.First().PaymentStatus,
                              OrderStatus = groupedData.First().OrderStatus,
                              OrderDate = groupedData.First().OrderDate
                          }).OrderByDescending(g => g.Index).ToListAsync();

        return data;
    }
}