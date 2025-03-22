using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository.AccountRepo
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(SonicStoreContext context) : base(context)
        {
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(int accountId, string newPassword)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Password = newPassword;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
        }
    }
}
