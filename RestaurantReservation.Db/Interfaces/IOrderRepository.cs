using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IOrderRepository
{
  Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId);

  Task<double> CalculateAverageOrderAmountAsync(int employeeId);
}