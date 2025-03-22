using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.CartRepo;
public interface ICartRepository
{
    List<Cart> GetCartIncludeInventoryAndProduct();
    Task<List<Cart>> GetCartIncludeInventoryAndProductHaveAddressCondition(List<int> cartIds);
    Task<Cart> GetCartItemByUser(int userId, int optionId);
    Task<List<Cart>> GetCartItemsByCustomerId(int customerId);
    Task<List<Cart>> GetCartItemsSessionAsync(List<int> cartIds);
    Task<List<Cart>> GetAllCartItemInCludeInfo(int customerId);
    Task<Cart> GetCartItemById(int id);
    Task<double?> GetUnitPrice(int storageId);
    void UpdateCartItem(Cart cartItem);
    void RemoveCartItemsRange(List<Cart> items);

    Task AddCartItem(Cart cartItem);
    Task SaveChangesAsync();
}
