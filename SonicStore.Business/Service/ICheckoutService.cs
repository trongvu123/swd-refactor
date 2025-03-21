using Microsoft.AspNetCore.Http;
using SonicStore.Business.Dto;
using SonicStore.Repository.Entity;

namespace SonicStore.Business.Service;
public interface ICheckoutService
{
    Task<CheckoutResponseDto> ProcessCheckoutAsync(CheckoutRequestDto request, HttpContext httpContext);
    Task<CheckoutResponseDto> ProcessBuyNowAsync(BuyNowRequestDto request, HttpContext httpContext);

    Task<int> SaveOrderAsync(Cart cart, Payment payment, Checkout checkout, StatusPayment statusPayment, StatusOrder statusOrder);
    Task ProcessPaymentCODSuccessAsync(List<int> cartIds, string userId, string paymentMethod);

    Task ProcessPaymentVnPAYSuccessAsync(List<int> cartIds, string userId, string paymentMethod);

    List<Cart> GetCartIncludeInventoryAndProduct();
    Task<List<Cart>> GetCartIncludeInventoryAndProductHaveAddressCondition(List<int> cartIds);

    Task<UserAddress> GetUserAddressActive(int userId);
}
