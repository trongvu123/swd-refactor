using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository {
    public class ProductRepository : IProductRepository
    {
        private readonly SonicStoreContext _context;

        public ProductRepository(SonicStoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int? id)
        {
            return await _context.Products
                .Include(p => p.Storages)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetBrandIdByProductIdAsync(int? id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Select(p => p.BrandId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int brandId, int excludeProductId)
        {
            return await _context.Brands
                .SelectMany(b => b.Products)
                .Include(p => p.Storages)
                .Where(p => p.BrandId == brandId && p.Id != excludeProductId)
                .Take(3)
                .ToListAsync();
        }

        public async Task<Product> GetProductFeedbackAsync(int? id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<string> GetUserAddressAsync(int userId)
        {
            return await _context.UserAddresses
                .Where(u => u.UserId == userId && u.Status == true)
                .Select(u => u.User_Address)
                .FirstOrDefaultAsync();
        }
    }
}
