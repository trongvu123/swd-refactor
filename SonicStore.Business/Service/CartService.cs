using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository;

namespace SonicStore.Business.Service;
public class CartService : BaseService<Cart>, ICartService
{
    public CartService(IRepository<Cart> repository) : base(repository)
    {
    }
}
