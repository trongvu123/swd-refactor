using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository;
public class PromotionRepository : IPromotionRepository
{
    private readonly SonicStoreContext _context;

    public PromotionRepository(SonicStoreContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPromotion(Promotion promotion)
    {
       await _context.Promotion.AddAsync(promotion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePromotion(int id)
    {
        var promotion = await GetPromotionById(id);
         _context.Promotion.Remove(promotion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Promotion>> GetListPromotionByUser(int userId)
    {
        return await _context.Promotion
                .Where(p => p.CreatedBy ==userId || p.UpdatedBy == userId)
                .Include(p => p.CreatedByUser)
                .Include(p => p.UpdatedByUser)
                .ToListAsync();
    }

    public async Task<Promotion> GetPromotionById(int id)
    {
        return await _context.Promotion.FindAsync(id);
    }

    public async Task<bool> UpdatePromotion(Promotion promotion)
    {
         _context.Promotion.Update(promotion);
        await _context.SaveChangesAsync();
        return true;
    }
}
