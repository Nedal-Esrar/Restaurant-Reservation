using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IOrderRepository
{
  Task CreateAsync(Order order);

  Task UpdateAsync(Order order);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);

  Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId);

  Task<double> CalculateAverageOrderAmountAsync(int employeeId);
}