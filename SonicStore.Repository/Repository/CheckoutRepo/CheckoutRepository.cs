﻿using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;

namespace SonicStore.Repository.Repository.CheckoutRepo;
public class CheckoutRepository : ICheckoutRepository
{
    private readonly SonicStoreContext _context;

    public CheckoutRepository(SonicStoreContext context)
    {
        _context = context;
    }

    public async Task<List<Cart>> GetCartItemsAsync(List<int> cartIds) =>
        await _context.OrderDetails
            .Include(o => o.Storage)
            .ThenInclude(o => o.Product)
            .Where(o => cartIds.Contains(o.Id))
            .ToListAsync();

    public async Task<int> SaveOrderAsync(Cart cart, Payment payment, Checkout checkout,
        StatusPayment statusPayment, StatusOrder statusOrder)
    {
        try
        {
            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Add cart (OrderDetail)
            _context.OrderDetails.Update(cart);
            await _context.SaveChangesAsync();

            // Add payment
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Add checkout (Order)
            checkout.CartId = cart.Id;
            checkout.PaymentId = payment.Id;
            _context.Orders.Add(checkout);
            await _context.SaveChangesAsync();

            // Add status payment
            statusPayment.Payment_id = payment.Id;
            _context.StatusPayments.Add(statusPayment);
            await _context.SaveChangesAsync();

            // Add status order
            statusOrder.OrderId = checkout.Id;
            _context.StatusOrders.Add(statusOrder);
            await _context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();

            return checkout.Id;
        }
        catch (Exception)
        {
            // Rollback will be handled automatically by disposing transaction
            throw;
        }
    }

    public async Task<int> GetOrderCountAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<int> GetMaxOrderIndexAsync()
    {
        return await _context.Orders.MaxAsync(o => o.index);
    }
}
