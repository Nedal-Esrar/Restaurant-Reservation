using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IMenuItemRepository
{
  Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}