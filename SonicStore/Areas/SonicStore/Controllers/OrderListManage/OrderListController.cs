using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SonicStore.Business.Service;
using SonicStore.Repository.Entity;

namespace SonicStore.Areas.SonicStore.Controllers.OrderListManage
{
    [Area("SonicStore")]
    //[Authorize(Roles = "sale")]
    public class OrderListController : Controller
    {
        private readonly IOrderListService _orderService;

        public OrderListController(IOrderListService orderService)
        {
            _orderService = orderService;
        }

        // GET: OrderListController
        [HttpGet("order-list")]
        public async Task<ActionResult> OrderListScreen()
        {
            var data = await _orderService.GetOrderListAsync();
            ViewBag.data = data;
            return View();
        }

    }
}
