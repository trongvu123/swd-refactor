using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.CartRepo;
public class CartRepository : ICartRepository
{
    private readonly SonicStoreContext _context;

    public CartRepository(SonicStoreContext context)
    {
        _context = context;
    }
    public List<Cart> GetCartIncludeInventoryAndProduct()
    {
        return _context.OrderDetails.Include(o => o.Storage).ThenInclude(o => o.Product).ToList();
    }

    public Task<List<Cart>> GetCartIncludeInventoryAndProductHaveAddressCondition(List<int> cartIds)
    {
        return _context.OrderDetails
                .Include(o => o.Storage)
                .ThenInclude(o => o.Product)
                .Where(o => cartIds.Contains(o.Id))
                .ToListAsync();
    }
    public async Task<List<Cart>> GetCartItemsByCustomerId(int customerId)
    {
        return await _context.OrderDetails
            .Where(o => o.CustomerId == customerId && o.Status == "cart")
            .ToListAsync();
    }

    public async Task<Cart> GetCartItemById(int id)
    {
        return await _context.OrderDetails
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Inventory> GetProductOptionByCartId(int id)
    {
        return await _context.OrderDetails
            .Include(o => o.Storage)
            .Where(o => o.Id == id)
            .Select(o => o.Storage)
            .FirstOrDefaultAsync();
    }

    public async Task<double?> GetUnitPrice(int storageId)
    {
        return await _context.OrderDetails
            .Include(c => c.Storage)
            .Where(c => c.StorageId == storageId)
            .Select(s => s.Storage.SalePrice) // Assuming Inventory has SalePrice property
            .FirstOrDefaultAsync();
    }

    public void UpdateCartItem(Cart cartItem)
    {
        _context.OrderDetails.Update(cartItem);
    }

    public void RemoveCartItemsRange(List<Cart> items)
    {
        _context.OrderDetails.RemoveRange(items);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetCartItemByUser(int userId, int optionId)
    {
        return await _context.OrderDetails.FirstOrDefaultAsync(od => od.StorageId == optionId && od.Status == "cart" && od.CustomerId == userId);
    }

    public async Task AddCartItem(Cart cartItem)
    {
        await _context.OrderDetails.AddAsync(cartItem);
    }
}
