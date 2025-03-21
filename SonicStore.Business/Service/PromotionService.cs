using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository;

namespace SonicStore.Business.Service;
public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository;

    public PromotionService(IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }
    public async Task<bool> AddPromotion(Promotion promotion)
    {
        await _promotionRepository.AddPromotion(promotion);
        return true;
    }

    public async Task<bool> DeletePromotion(int id)
    {
        await _promotionRepository.DeletePromotion(id);
        return true;
    }

    public async Task<Promotion> FindPromotionById(int id)
    {
        return await _promotionRepository.GetPromotionById(id);
    }

    public async Task<List<Promotion>> GetListPromotionByUser(int userId)
    {
        return await _promotionRepository.GetListPromotionByUser(userId);
    }

    public async Task<bool> UpdatePromotion(Promotion promotion)
    {
        return await _promotionRepository.UpdatePromotion(promotion);
    }
}
