using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SonicStore.Business.Service.ProductService;
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

            var productFeedback = await _productService.GetProductFeedbackAsync(id);
            var productFound = await _productService.GetProductByIdAsync(id);
            var brandId = productFound.BrandId;
            var listProductRelate = await _productService.GetRelatedProductsAsync(brandId, id);
            var product = await _productService.GetProductByIdAsync(id);
    
            ViewBag.listProductRelate = listProductRelate;
            ViewBag.product = product;
            ViewBag.productFeedback = productFeedback;

            return View(listProductRelate);
        }
    }
}
