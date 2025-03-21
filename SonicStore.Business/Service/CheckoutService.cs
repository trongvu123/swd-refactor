using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SonicStore.Business.Dto;
using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository;

namespace SonicStore.Business.Service;
public class CheckoutService : ICheckoutService
{
    private readonly ICheckoutRepository _checkoutRepository;
    private readonly IVnPayService _vnPayService;
    private readonly ICartRepository _cartRepository;
    private readonly IUserAddressRepository _userAddressRepository;

    public CheckoutService(ICheckoutRepository checkoutRepository, IVnPayService vnPayService, ICartRepository cartRepository, IUserAddressRepository userAddressRepository)
    {
        _checkoutRepository = checkoutRepository;
        _vnPayService = vnPayService;
        _cartRepository = cartRepository; 
        _userAddressRepository = userAddressRepository;
    }

    public async Task<CheckoutResponseDto> ProcessCheckoutAsync(CheckoutRequestDto request, HttpContext httpContext)
    {
        // Validation
        if (request == null || request.CartIds == null || !request.CartIds.Any())
            return new CheckoutResponseDto { Success = false, Message = "Invalid cart items" };

        if (string.IsNullOrEmpty(request.PaymentMethod))
            return new CheckoutResponseDto { Success = false, Message = "Payment method is required" };

        var userJson = httpContext.Session.GetString("user");
        if (string.IsNullOrEmpty(userJson))
            return new CheckoutResponseDto { Success = false, Message = "User session not found" };

        var user = JsonConvert.DeserializeObject<User>(userJson);
        var cartItems = await _checkoutRepository.GetCartItemsAsync(request.CartIds);

        if (!cartItems.Any())
            return new CheckoutResponseDto { Success = false, Message = "No items to checkout" };

        double totalPrice = (double)cartItems.Sum(item => item.Quantity * item.Storage.SalePrice);

        if (request.PaymentMethod.ToLower() == "cod")
        {
            await ProcessCODPayment(user, cartItems);
            return new CheckoutResponseDto
            {
                Success = true,
                Message = "Đơn hàng của bạn đang chờ duyệt",
                TotalPrice = (decimal)totalPrice
            };
        }
        else
        {
            var listJson = JsonConvert.SerializeObject(request.CartIds);
            httpContext.Session.SetString("listCheckout", listJson);

            var vnPayModel = new VnPayRequestModel
            {
                Fullname = user.FullName,
                Description = "Checkout payment",
                Amount = (double)totalPrice,
                CreatedDate = DateTime.Now,
                OrderId = new Random().Next(1000, 100000)
            };

            var paymentUrl = _vnPayService.CreatePaymentURL(httpContext, vnPayModel);
            return new CheckoutResponseDto
            {
                Success = true,
                PaymentUrl = paymentUrl,
                TotalPrice = (decimal)totalPrice
            };
        }
    }

    public async Task<CheckoutResponseDto> ProcessBuyNowAsync(BuyNowRequestDto request, HttpContext httpContext)
    {
        // Validation
        if (request == null || request.StorageId <= 0)
            return new CheckoutResponseDto { Success = false, Message = "Invalid product selection" };

        if (request.ReceiveTypeId != 1 && request.ReceiveTypeId != 2)
            return new CheckoutResponseDto { Success = false, Message = "Invalid receive type" };

        var userJson = httpContext.Session.GetString("user");
        if (string.IsNullOrEmpty(userJson))
            return new CheckoutResponseDto { Success = false, Message = "User session not found" };

        var user = JsonConvert.DeserializeObject<User>(userJson);
        var storage = await _checkoutRepository.GetStorageAsync(request.StorageId);

        if (storage == null || storage.quantity <= 0)
            return new CheckoutResponseDto { Success = false, Message = "Product out of stock" };

        var userAddress = await _checkoutRepository.GetUserAddressAsync(user.Id.ToString());
        if (userAddress == null)
            return new CheckoutResponseDto { Success = false, Message = "User address not found" };

        // Update address
        userAddress.User_Address = $"{request.Xa}, {request.Huyen}, {request.Tinh}";
        await _checkoutRepository.UpdateUserAddressAsync(userAddress);

        if (request.ReceiveTypeId == 1) // COD
        {
            var cart = new Cart
            {
                CustomerId = user.Id,
                AddressId = userAddress.Id,
                Quantity = 1,
                Price = storage.SalePrice,
                StorageId = storage.Id,
                Status = "bill"
            };

            await ProcessCODPayment(user, new List<Cart> { cart });
            return new CheckoutResponseDto
            {
                Success = true,
                Message = "Đơn hàng của bạn đang chờ duyệt",
                TotalPrice = (decimal)storage.SalePrice
            };
        }
        else // VNPAY
        {
            var orderJson = JsonConvert.SerializeObject(request);
            httpContext.Session.SetString("order", orderJson);

            var vnPayModel = new VnPayRequestModel
            {
                Fullname = user.FullName,
                Description = "Buy now payment",
                Amount = (double)storage.SalePrice,
                CreatedDate = DateTime.Now,
                OrderId = new Random().Next(1000, 100000)
            };

            var paymentUrl = _vnPayService.CreatePaymentURL(httpContext, vnPayModel);
            return new CheckoutResponseDto
            {
                Success = true,
                PaymentUrl = paymentUrl,
                TotalPrice = (decimal)storage.SalePrice
            };
        }
    }

    public async Task<int> SaveOrderAsync(Cart cart, Payment payment, Checkout checkout,
        StatusPayment statusPayment, StatusOrder statusOrder)
    {
        return await _checkoutRepository.SaveOrderAsync(cart, payment, checkout,
            statusPayment, statusOrder);
    }

    public async Task ProcessPaymentCODSuccessAsync(List<int> cartIds, string userId, string paymentMethod)
    {
        try
        {
            var cartItems = await _checkoutRepository.GetCartItemsAsync(cartIds);
            if (cartItems == null || !cartItems.Any())
                throw new Exception("No cart items found to process payment.");

            // Tính index cho đơn hàng mới
            int orderCount = await _checkoutRepository.GetOrderCountAsync();
            int maxIndex = orderCount > 0 ? await _checkoutRepository.GetMaxOrderIndexAsync() : 0;
            int index = maxIndex + 1;

            foreach (var cart in cartItems)
            {
                cart.Status = "bill";

                var payment = new Payment
                {
                    TotalPrice = cart.Quantity * cart.Storage.SalePrice, // Ép kiểu decimal để đồng bộ
                    PaymentMethod = paymentMethod,
                    TransactionDate = DateTime.Now
                };

                var checkout = new Checkout
                {
                    OrderDate = DateTime.Now,
                    SaleId = int.Parse(userId), // Giữ string nếu User.Id là string
                    CartId = cart.Id,
                    PaymentId = payment.Id, // Sẽ được gán trong SaveOrderAsync
                    index = index++ // Tăng index cho mỗi đơn hàng
                };

                var statusPayment = new StatusPayment
                {
                    Type = "Đã thanh toán",
                    UpdateAt = DateTime.Now,
                    UpdateBy =int.Parse(userId), // Giữ string nếu User.Id là string
                    CreateAt = DateTime.Now,
                    CreateBy = int.Parse(userId),
                    Payment_id = payment.Id // Sẽ được gán trong SaveOrderAsync
                };

                var statusOrder = new StatusOrder
                {
                    Type = "Chờ duyệt",
                    UpdateAt = DateTime.Now,
                    UpdateBy = int.Parse(userId),
                    CreateAt = DateTime.Now,
                    CreateBy = int.Parse(userId),
                    OrderId = checkout.Id // Sẽ được gán trong SaveOrderAsync
                };

                await _checkoutRepository.SaveOrderAsync(cart, payment, checkout, statusPayment, statusOrder);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing payment success: {ex.Message}", ex);
        }
    }




    private async Task ProcessCODPayment(User user, List<Cart> cartItems)
    {
        try
        {
            foreach (var cart in cartItems)
            {
                var storage = await _checkoutRepository.GetStorageAsync(cart.StorageId);
                if (storage != null && storage.quantity >= cart.Quantity)
                {
                    storage.quantity -= (int)cart.Quantity;
                    await _checkoutRepository.UpdateStorageAsync(storage);

                    cart.Status = "bill";
                    cart.CustomerId = user.Id;

                    var payment = new Payment
                    {
                        TotalPrice = cart.Quantity * cart.Price,
                        PaymentMethod = "COD",
                        TransactionDate = DateTime.Now
                    };

                    var checkout = new Checkout
                    {
                        OrderDate = DateTime.Now,
                        SaleId = user.Id,
                        index = await _checkoutRepository.GetOrderCountAsync() + 1
                    };

                    var statusPayment = new StatusPayment
                    {
                        Type = "Chưa thanh toán",
                        UpdateAt = DateTime.Now,
                        UpdateBy = user.Id,
                        CreateAt = DateTime.Now,
                        CreateBy = user.Id
                    };

                    var statusOrder = new StatusOrder
                    {
                        Type = "Chờ duyệt",
                        UpdateAt = DateTime.Now,
                        UpdateBy = user.Id,
                        CreateAt = DateTime.Now,
                        CreateBy = user.Id
                    };

                    await _checkoutRepository.SaveOrderAsync(cart, payment, checkout,
                        statusPayment, statusOrder);
                }
                else
                {
                    throw new Exception($"Insufficient stock for item with StorageId: {cart.StorageId}");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing COD payment: {ex.Message}", ex);
        }
    }

    public async Task ProcessPaymentVnPAYSuccessAsync(List<int> cartIds, string userId, string paymentMethod)
    {
        try
        {
            var cartItems = await _checkoutRepository.GetCartItemsAsync(cartIds);
            if (cartItems == null || !cartItems.Any())
                throw new Exception("No cart items found to process payment.");

            // Tính index cho đơn hàng mới
            int orderCount = await _checkoutRepository.GetOrderCountAsync();
            int maxIndex = orderCount > 0 ? await _checkoutRepository.GetMaxOrderIndexAsync() : 0;
            int index = maxIndex + 1;

            foreach (var cart in cartItems)
            {
                cart.Status = "bill";

                var payment = new Payment
                {
                    TotalPrice = cart.Quantity * cart.Storage.SalePrice, // Ép kiểu decimal để đồng bộ
                    PaymentMethod = paymentMethod,
                    TransactionDate = DateTime.Now
                };

                var checkout = new Checkout
                {
                    OrderDate = DateTime.Now,
                    SaleId = int.Parse(userId), // Giữ string nếu User.Id là string
                    CartId = cart.Id,
                    PaymentId = payment.Id, // Sẽ được gán trong SaveOrderAsync
                    index = index++ // Tăng index cho mỗi đơn hàng
                };

                var statusPayment = new StatusPayment
                {
                    Type = "Đã thanh toán",
                    UpdateAt = DateTime.Now,
                    UpdateBy = int.Parse(userId), // Giữ string nếu User.Id là string
                    CreateAt = DateTime.Now,
                    CreateBy = int.Parse(userId),
                    Payment_id = payment.Id // Sẽ được gán trong SaveOrderAsync
                };

                var statusOrder = new StatusOrder
                {
                    Type = "Chờ duyệt",
                    UpdateAt = DateTime.Now,
                    UpdateBy = int.Parse(userId),
                    CreateAt = DateTime.Now,
                    CreateBy = int.Parse(userId),
                    OrderId = checkout.Id // Sẽ được gán trong SaveOrderAsync
                };

                await _checkoutRepository.SaveOrderAsync(cart, payment, checkout, statusPayment, statusOrder);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing payment success: {ex.Message}", ex);
        }
    }

    public List<Cart> GetCartIncludeInventoryAndProduct()
    {
        return _cartRepository.GetCartIncludeInventoryAndProduct();
    }

    public Task<List<Cart>> GetCartIncludeInventoryAndProductHaveAddressCondition(List<int> cartIds)
    {
        return _cartRepository.GetCartIncludeInventoryAndProductHaveAddressCondition(cartIds);
    }

    public Task<UserAddress> GetUserAddressActive(int userId)
    {
        return _userAddressRepository.GetUserAddressActive(userId);
    }
}
