﻿using SonicStore.Repository.Entity;

namespace SonicStore.Business.Service.CartService;
public interface ICartService
{
    Task<int> UpdateCartItemQuantity(int? id, string? quantity);
    Task RemoveAllCartItems(int customerId);

    Task<Cart> GetCartItemByUser(int userId, int optionId);

    Task<List<Cart>> GetAllCartIncludeInfo(int customerId);

    Task<Inventory> GetInventoryOption(int id);

    Task<UserAddress> GetUserAddress(int userId);

    Task AddCartItem(Cart cart);
    Task SaveChange();
}
