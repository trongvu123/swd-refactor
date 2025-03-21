using SonicStore.Business.Dto;

namespace SonicStore.Business.Service;
public interface IOrderListService
{
    Task<List<OrderListViewModel>> GetOrderListAsync();
}