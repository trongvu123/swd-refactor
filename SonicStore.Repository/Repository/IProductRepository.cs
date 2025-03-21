using SonicStore.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository;
public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int? id);
    Task<int> GetBrandIdByProductIdAsync(int? id);
    Task<List<Product>> GetRelatedProductsAsync(int brandId, int excludeProductId);
    Task<Product> GetProductFeedbackAsync(int? id);
    Task<string> GetUserAddressAsync(int userId);
}
