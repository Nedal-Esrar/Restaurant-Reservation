using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
  private readonly RestaurantReservationDbContext _context;

  public OrderItemRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(OrderItem orderItem)
  {
    if (orderItem is null) throw new ArgumentNullException(nameof(orderItem));

    await _context.OrderItems.AddAsync(orderItem);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(OrderItem orderItem)
  {
    if (orderItem is null) throw new ArgumentNullException(nameof(orderItem));

    if (!await IsExistAsync(orderItem.OrderItemId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("OrderItem", orderItem.OrderItemId));

    _context.OrderItems.Update(orderItem);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.OrderItems.AnyAsync(oi => oi.OrderItemId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var orderItem = await _context.OrderItems.FindAsync(id) ??
                    throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("OrderItem", id));

    _context.OrderItems.Remove(orderItem);

    await _context.SaveChangesAsync();
  }
}