using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.InventoryRepo;
public interface IInventoryRepository
{
    Task<Inventory> GetInventoryById(int id);
}
