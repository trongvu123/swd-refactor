using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository.UserRepo
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(SonicStoreContext context) : base(context)
        {
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByEmailOrPhoneAsync(string email, string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email || u.Phone == phone);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<User> GetUserByAccountIdAsync(int accountId)
        {
            return await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task<bool> CheckExistingUserAsync(string email, string phone)
        {
            return await _context.Users.AnyAsync(u => u.Email == email || u.Phone == phone);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
