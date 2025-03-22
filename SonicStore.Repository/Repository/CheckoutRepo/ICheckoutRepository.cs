using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.CheckoutRepo;
public interface ICheckoutRepository
{

    Task<List<Cart>> GetCartItemsAsync(List<int> cartIds);
    Task<int> SaveOrderAsync(Cart cart, Payment payment, Checkout checkout,
        StatusPayment statusPayment, StatusOrder statusOrder);
    Task<int> GetOrderCountAsync();
    Task<int> GetMaxOrderIndexAsync();
}
