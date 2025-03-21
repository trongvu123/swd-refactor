using SonicStore.Repository.Entity;
using System.Threading.Tasks;

namespace SonicStore.Repository.Repository
{
    public abstract class BaseRepository
    {
        protected readonly SonicStoreContext _context;

        protected BaseRepository(SonicStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
