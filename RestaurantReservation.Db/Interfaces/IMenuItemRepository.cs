using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IMenuItemRepository : IRepository<MenuItem>
{
  Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}