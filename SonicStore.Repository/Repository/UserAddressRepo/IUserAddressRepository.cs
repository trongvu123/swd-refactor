using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.UserAddressRepo;
public interface IUserAddressRepository
{
    Task<UserAddress> GetUserAddressActive(int userId);
    Task<UserAddress> GetUserAddressByIdAsync(int id);
    Task<List<UserAddress>> GetUserAddressesByUserIdAsync(int userId);
    Task<bool> AddUserAddressAsync(UserAddress userAddress);
    Task<bool> UpdateUserAddressAsync(UserAddress userAddress);
}
