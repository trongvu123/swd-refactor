using SonicStore.Repository.Entity;

namespace SonicStore.Business.Service.PromotionService;
public interface IPromotionService
{
    Task<List<Promotion>> GetListPromotionByUser(int userId);
    Task<bool> AddPromotion(Promotion promotion);
    Task<bool> UpdatePromotion(Promotion promotion);
    Task<bool> DeletePromotion(int id);

    Task<Promotion> FindPromotionById(int id);
}
