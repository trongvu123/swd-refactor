using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.UserAddressRepo;
public class UserAddressRepository : BaseRepository, IUserAddressRepository
{
    private readonly SonicStoreContext _context;

    public UserAddressRepository(SonicStoreContext context) : base(context)
    {
        _context = context;
    }
    public Task<UserAddress> GetUserAddressActive(int userId)
    {
        return _context.UserAddresses
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Status);
    }
    public async Task<UserAddress> GetUserAddressByIdAsync(int id)
    {
        return await _context.UserAddresses.FindAsync(id);
    }

    public async Task<List<UserAddress>> GetUserAddressesByUserIdAsync(int userId)
    {
        return await _context.UserAddresses
            .Where(ua => ua.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> AddUserAddressAsync(UserAddress userAddress)
    {
        await _context.UserAddresses.AddAsync(userAddress);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserAddressAsync(UserAddress userAddress)
    {
        _context.UserAddresses.Update(userAddress);
        await _context.SaveChangesAsync();
        return true;
    }
}
