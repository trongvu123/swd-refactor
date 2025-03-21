using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByPhoneAsync(string phone);
        Task<User> GetUserByAccountIdAsync(int accountId);
        Task<User> GetUserByEmailOrPhoneAsync(string email, string phone);
        Task<bool> CheckExistingUserAsync(string email, string phone);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
    }
}
