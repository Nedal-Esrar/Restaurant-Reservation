using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IMenuItemRepository : IRepository<MenuItem>
{
  Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}