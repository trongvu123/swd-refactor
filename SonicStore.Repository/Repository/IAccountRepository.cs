using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(int id);
        Task<Account> GetAccountByUsernameAsync(string username);
        Task<bool> AddAccountAsync(Account account);
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> UpdatePasswordAsync(int accountId, string newPassword);
        Task<bool> CheckUsernameExistsAsync(string username);
    }
}
