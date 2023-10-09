using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
  private readonly RestaurantReservationDbContext _context;

  public MenuItemRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(MenuItem menuItem)
  {
    if (menuItem is null) throw new ArgumentNullException(nameof(menuItem));

    await _context.MenuItems.AddAsync(menuItem);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(MenuItem menuItem)
  {
    if (menuItem is null) throw new ArgumentNullException(nameof(menuItem));

    if (!await IsExistAsync(menuItem.ItemId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("MenuItem", menuItem.ItemId));

    _context.MenuItems.Update(menuItem);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.MenuItems.AnyAsync(mi => mi.ItemId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var menuItem = await _context.MenuItems.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("MenuItem", id));

    _context.MenuItems.Remove(menuItem);

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
  {
    if (!await _context.Reservations.AnyAsync(r => r.ReservationId == reservationId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservationId));
    }
    
    return await _context.OrderItems
      .Where(oi => oi.Order.ReservationId == reservationId)
      .Select(oi => oi.Item)
      .Distinct()
      .ToListAsync();
  }
}