using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository.CartRepo;
using SonicStore.Repository.Repository.InventoryRepo;
using SonicStore.Repository.Repository.UserAddressRepo;

namespace SonicStore.Business.Service.CartService;
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserAddressRepository _userAddressRepository;
    private readonly IInventoryRepository _inventoryRepository;
    public CartService(ICartRepository cartRepository, IUserAddressRepository userAddressRepository, IInventoryRepository inventoryRepository)
    {
        _cartRepository = cartRepository;
        _userAddressRepository = userAddressRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task<int> UpdateCartItemQuantity(int? id, string? quantity)
    {
        if (!id.HasValue || string.IsNullOrEmpty(quantity)) return 0;

        var cartItem = await _cartRepository.GetCartItemById(id.Value);
        if (cartItem == null) return 0;

        var unitPrice = await _cartRepository.GetUnitPrice(cartItem.StorageId) ?? 0;
        var productOption = await _inventoryRepository.GetProductOptionByCartId(id.Value);

        if (int.TryParse(quantity, out var quantityInput))
        {
            cartItem.Quantity = quantityInput;
            cartItem.Price = quantityInput * unitPrice;
            _cartRepository.UpdateCartItem(cartItem);
            await _cartRepository.SaveChangesAsync();
            return 1;
        }

        if (quantity == "down")
        {
            if (cartItem.Quantity <= 1)
            {
                cartItem.Quantity = 1;
                _cartRepository.UpdateCartItem(cartItem);
                await _cartRepository.SaveChangesAsync();
                return 2;
            }

            cartItem.Quantity -= 1;
            cartItem.Price -= unitPrice;
            _cartRepository.UpdateCartItem(cartItem);
            await _cartRepository.SaveChangesAsync();
            return 1;
        }

        // Assuming Inventory has a Quantity property for available stock
        if (cartItem.Quantity > productOption.quantity - 1)
        {
            return 3;
        }

        cartItem.Quantity += 1;
        cartItem.Price += unitPrice;
        _cartRepository.UpdateCartItem(cartItem);
        await _cartRepository.SaveChangesAsync();
        return 1;
    }

    public async Task RemoveAllCartItems(int customerId)
    {
        var cartItems = await _cartRepository.GetCartItemsByCustomerId(customerId);
        _cartRepository.RemoveCartItemsRange(cartItems);
        await _cartRepository.SaveChangesAsync();
    }

    public Task<Cart> GetCartItemByUser(int userId)
    {
        return _cartRepository.GetCartItemById(userId);
    }

    public Task<Inventory> GetInventoryOption(int id)
    {
        return _inventoryRepository.GetInventoryById(id);
    }

    public Task<UserAddress> GetUserAddress(int userId)
    {
        return _userAddressRepository.GetUserAddressActive(userId);
    }

    public Task<Cart> GetCartItemByUser(int userId, int optionId)
    {
        return _cartRepository.GetCartItemByUser(userId, optionId);
    }

    public async Task AddCartItem(Cart cart)
    {
        await _cartRepository.AddCartItem(cart);
    }

    public async Task SaveChange()
    {
        await _cartRepository.SaveChangesAsync();
    }

    public Task<List<Cart>> GetAllCartIncludeInfo(int customerId)
    {
        return _cartRepository.GetAllCartItemInCludeInfo(customerId);
    }
}

