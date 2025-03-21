using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using SonicStore.Repository.Entity;
using SonicStore.Areas.SonicStore.Dtos;
using Microsoft.AspNetCore.Cors.Infrastructure;
using SonicStore.Business.Service.CartService;

namespace SonicStore.Areas.SonicStore.Controllers.CartManage
{
    [Authorize(Roles = "customer")]
    [Area("SonicStore")]
    public class CartController : Controller
    {
        private readonly SonicStoreContext _context;
        private readonly ICartService _cartService;

        public CartController(SonicStoreContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> CartScreen()
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            var user = _context.Users.FirstOrDefault();
            var addressUser = await _cartService.GetUserAddress(userSession.Id);
            ViewBag.AddressUser = addressUser;
            ViewBag.user = user;
            return View();
        }

        [AllowAnonymous]
        [HttpPost("add-item")]
        public async Task<JsonResult> AddProductToCart(int id)
        {
            var userJson = HttpContext.Session.GetString("user");
            if (userJson == null)
            {
                return Json(new { status = 401, redirectUrl = "/SonicStore/Login/login" });
            }
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            var productItem = await _cartService.GetCartItemByUser(userSession.Id,id);
            var productOption = await _cartService.GetInventoryOption(id);
            var userAddressId = await _cartService.GetUserAddress(userSession.Id);
            int check = 1;
            if (productItem == null)
            {
                var cart = new Cart
                {
                    StorageId = id,
                    CustomerId = userSession.Id,
                    Quantity = 1,
                    Price = productOption.SalePrice,
                    AddressId = userAddressId.Id,
                    Status = "cart"
                };
                await _cartService.AddCartItem(cart);
                await _cartService.SaveChange();
            }
            else
            {
                productItem.Quantity += 1;
                productItem.Price += productOption.SalePrice;
                await _cartService.SaveChange();
                if (productItem.Quantity > productOption.quantity - 1)
                {
                    check = 2;
                }
                else if (productOption.quantity == 0)
                {
                    check = 3;
                }
                else
                {
                    await _cartService.SaveChange();

                }
            }

            return Json(new { status = check });
        }

        [HttpGet("loaddataCart")]
        public async Task<JsonResult> loadData()
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
			var listJson = HttpContext.Session.GetString("listCheckout");

			List<int> listSession = new List<int>();
			if (!string.IsNullOrEmpty(listJson))
			{
				listSession = JsonConvert.DeserializeObject<List<int>>(listJson) ?? new List<int>();
			}
			var listCartItem = await _context.OrderDetails.Where(od => od.CustomerId == userSession.Id && od.Status=="cart" && od.Storage.Product.Status == true).Include(u => u.User)
	                            .Include(u => u.UserAddress)
	                            .Include(s => s.Storage)
	                            .ThenInclude(p => p.Product)
                                .Select(c=> new
                                {
                                    c.Id,
                                    c.Price,
                                    c.StorageId,
                                    c.Storage.Product.Name,
									c.Storage.Product.Image,
									c.Storage.SalePrice,
									StorageQuantity = c.Storage.quantity,
                                    c.Quantity,
									isCheck = listSession.Contains(c.Id)
								})
	                            .ToListAsync();
            return Json(new { data = listCartItem , status = true});
		}
        [HttpPost("update-quantity")]
        public async Task<JsonResult> UpdateQuantity(string? quantity, int? id, int? all)
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);

            int status = 0;

            if (id.HasValue && !string.IsNullOrEmpty(quantity))
            {
                status = await _cartService.UpdateCartItemQuantity(id, quantity);
            }

            if (all.HasValue)
            {
                await _cartService.RemoveAllCartItems(userSession.Id);
            }

            return Json(new { status = status });
        }
        [HttpPost("buy-product")]
        public async Task<IActionResult> BuyProduct([FromBody] List<int> listProduct)
        {

            if (listProduct == null || listProduct.Count == 0)
            {
                return BadRequest("No products selected.");
            }

            var listProductJson = JsonConvert.SerializeObject(listProduct);
            HttpContext.Session.SetString("listCheckout", listProductJson);

            return Ok(new { redirectUrl = Url.Action("CheckoutScreen", "Checkout") });
        }
    }
}
