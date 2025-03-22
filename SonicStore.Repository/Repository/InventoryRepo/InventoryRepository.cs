using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.InventoryRepo;
public class InventoryRepository : IInventoryRepository
{
    private readonly SonicStoreContext _context;

    public InventoryRepository(SonicStoreContext context)
    {
        _context = context;
    }
    public async Task<Inventory> GetInventoryById(int id)
    {
        return await _context.Storages.Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Inventory> GetProductOptionByCartId(int id)
    {
        return await _context.OrderDetails
            .Include(o => o.Storage)
            .Where(o => o.Id == id)
            .Select(o => o.Storage)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateStorageAsync(Inventory inventory)
    {
        try
        {
            _context.Storages.Update(inventory);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
