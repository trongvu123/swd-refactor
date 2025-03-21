using SonicStore.Business.Dto;

namespace SonicStore.Business.Service.OrderService;
public interface IOrderListService
{
    Task<List<OrderListViewModel>> GetOrderListAsync();
}