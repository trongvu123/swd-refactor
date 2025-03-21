using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository.ProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicStore.Business.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductByIdAsync(int? id)
        {
            if (!id.HasValue) return null;
            return await _productRepository.GetProductByIdAsync(id.Value);
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int? brandId, int? excludeProductId)
        {
            if (!brandId.HasValue || !excludeProductId.HasValue) return new List<Product>();
            return await _productRepository.GetRelatedProductsAsync(brandId.Value, excludeProductId.Value);
        }

        public async Task<Product> GetProductFeedbackAsync(int? id)
        {
            if (!id.HasValue) return null;
            return await _productRepository.GetProductFeedbackAsync(id.Value);
        }

        public async Task<string> GetUserAddressAsync(int userId)
        {
            return await _productRepository.GetUserAddressAsync(userId);
        }
    }

}
