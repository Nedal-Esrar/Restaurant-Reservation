using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
  public MenuItemRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
  {
    if (!await Context.Reservations.AnyAsync(r => r.Id == reservationId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservationId));
    }
    
    return await Context.OrderItems
      .Where(oi => oi.Order.ReservationId == reservationId)
      .Select(oi => oi.Item)
      .Distinct()
      .ToListAsync();
  }
}