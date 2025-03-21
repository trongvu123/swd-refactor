using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Business.Service.AccountService
{
    public interface ILoginService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<(bool success, string role, int userId)> ValidateUserAsync(string username, string password);
        Task<User> GetUserInfoAsync(int userId);
    }
}
