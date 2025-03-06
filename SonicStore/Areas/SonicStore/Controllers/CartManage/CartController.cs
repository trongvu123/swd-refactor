using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using SonicStore.Repository.Entity;
using SonicStore.Areas.SonicStore.Dtos;

namespace SonicStore.Areas.SonicStore.Controllers.CartManage
{
    [Authorize(Roles = "customer")]
    [Area("SonicStore")]
    public class CartController : Controller
    {
        private readonly SonicStoreContext _context;

        public CartController(SonicStoreContext context)
        {
            _context = context;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> CartScreen()
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            var user = _context.Users.FirstOrDefault();
            var addressUser = await _context.UserAddresses.Where(u => u.UserId == userSession.Id && u.Status == true).Select(a => a.User_Address).FirstOrDefaultAsync();
            ViewBag.AddressUser = addressUser;
            ViewBag.user = user;
            return View();
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
            int status = 0;
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            var allItem = await _context.OrderDetails.Where(o => o.CustomerId == userSession.Id && o.Status == "cart").ToListAsync();
            var productOption = await _context.OrderDetails.Include(o => o.Storage).Where(o => o.Id == id).Select(o => o.Storage).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(quantity))
            {
                var cartItem = await _context.OrderDetails.Where(c => c.Id == id).FirstOrDefaultAsync();
                var unitPrice = await _context.OrderDetails.Where(c => cartItem.StorageId == c.Storage.Id).Include(c => c.Storage).Select(s => s.Storage.SalePrice).FirstOrDefaultAsync();

                if (int.TryParse(quantity, out var quantityInput))
                {
                    cartItem.Quantity = quantityInput;
                    cartItem.Price = quantityInput * unitPrice;
                    status = 1;
                    _context.OrderDetails.Update(cartItem);
                }
                else
                {
                    if (quantity == "down")
                    {
                        if (cartItem.Quantity <= 1)
                        {
                            status = 2;
                            cartItem.Quantity = 1;
                            _context.OrderDetails.Update(cartItem);
                        }                     
                        else
                        {
                            cartItem.Quantity -= 1;
                            cartItem.Price -= unitPrice;
                            status = 1;
                            _context.OrderDetails.Update(cartItem);
                        }
                    }
                    else
                    {
                        if (cartItem.Quantity > productOption.quantity - 1)
                        {
                            status = 3;
                        }
                        else
                        {
                            cartItem.Quantity += 1;
                            cartItem.Price += unitPrice;
                            status = 1;
                            _context.OrderDetails.Update(cartItem);
                        }
                    }
                }
            }
            if (all.HasValue)
            {
                _context.OrderDetails.RemoveRange(allItem);

            }
            await _context.SaveChangesAsync();
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
