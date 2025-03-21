using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SonicStore.Business.Service;
using SonicStore.Repository.Entity;

namespace SonicStore.Areas.SonicStore.Controllers.ProductDetailManage
{
    [Area("SonicStore")]
    public class ProductDetailController : Controller
    {
        private readonly SonicStoreContext _context;
        private readonly IProductService _productService;
        public ProductDetailController(SonicStoreContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        [HttpGet("product-detail/{id?}")]
        public async Task<IActionResult> ProductDetailScreen(int? id, int? storageId)
        {
            //var userJson = HttpContext.Session.GetString("user");
            //var userSession = JsonConvert.DeserializeObject<User>(userJson);

            var productFeedback = await _productService.GetProductFeedbackAsync(id);
            var brandId = await _context.Products.Where(p => p.Id == id).Select(p => p.BrandId).FirstOrDefaultAsync(); // Temporary direct DB access
            var listProductRelate = await _productService.GetRelatedProductsAsync(brandId, id);
            var product = await _productService.GetProductByIdAsync(id);
            //var address = await _productService.GetUserAddressAsync(userSession.Id);

            //string[] arr = address?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            //ViewBag.AddressParts = arr;
            ViewBag.listProductRelate = listProductRelate;
            ViewBag.product = product;
            //ViewBag.userSession = userSession;
            ViewBag.productFeedback = productFeedback;

            return View(listProductRelate);
        }
    }
}
