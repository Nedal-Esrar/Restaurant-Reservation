using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IMenuItemRepository
{
  Task CreateAsync(MenuItem menuItem);

  Task UpdateAsync(MenuItem menuItem);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);

  Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}