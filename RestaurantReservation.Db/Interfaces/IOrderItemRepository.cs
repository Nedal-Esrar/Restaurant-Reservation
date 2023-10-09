using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IOrderItemRepository
{
  Task CreateAsync(OrderItem orderItem);

  Task UpdateAsync(OrderItem orderItem);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);
}