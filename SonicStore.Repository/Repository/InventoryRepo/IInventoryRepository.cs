using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.InventoryRepo;
public interface IInventoryRepository
{
    Task<Inventory> GetInventoryById(int id);
    Task UpdateStorageAsync (Inventory inventory);
    Task<Inventory> GetProductOptionByCartId(int id);
}
