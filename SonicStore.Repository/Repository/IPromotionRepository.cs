

using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository;
public interface IPromotionRepository 
{
    Task<List<Promotion>> GetListPromotionByUser(int userId);
    Task<bool> AddPromotion(Promotion promotion);
    Task<bool> UpdatePromotion(Promotion promotion);
    Task<bool> DeletePromotion(int id);

    Task<Promotion> GetPromotionById(int id);
}
