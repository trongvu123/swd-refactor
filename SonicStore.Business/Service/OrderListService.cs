using SonicStore.Business.Dto;
using SonicStore.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicStore.Business.Service;
public class OrderListService : IOrderListService
{
    private readonly IOrderRepository _orderRepository;

    public OrderListService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderListViewModel>> GetOrderListAsync()
    {
        var data = await _orderRepository.GetOrderListAsync();
        return data.Select(d => new OrderListViewModel
        {
            Index = d.Index,
            CusName = d.CusName,
            PaymentStatus = d.PaymentStatus,
            OrderStatus = d.OrderStatus,
            OrderDate = (DateTime)d.OrderDate
        }).ToList();
    }
}
