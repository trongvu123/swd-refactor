using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.CheckoutRepo;
public interface ICheckoutRepository
{
    Task<User> GetUserAsync(string userId);
    Task<UserAddress> GetUserAddressAsync(string userId);
    Task<List<Cart>> GetCartItemsAsync(List<int> cartIds);
    Task<Inventory> GetStorageAsync(int storageId);
    Task<int> SaveOrderAsync(Cart cart, Payment payment, Checkout checkout,
        StatusPayment statusPayment, StatusOrder statusOrder);
    Task UpdateStorageAsync(Inventory storage);
    Task UpdateUserAddressAsync(UserAddress address);
    Task<int> GetOrderCountAsync();
    Task<int> GetMaxOrderIndexAsync();
}
