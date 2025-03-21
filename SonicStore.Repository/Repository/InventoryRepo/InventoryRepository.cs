using Microsoft.EntityFrameworkCore;
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
}
