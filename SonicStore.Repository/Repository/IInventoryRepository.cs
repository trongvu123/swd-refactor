using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository;
public interface IInventoryRepository
{
    Task<Inventory> GetInventoryById(int id);
}
