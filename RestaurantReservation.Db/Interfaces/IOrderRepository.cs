using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
  Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId);

  Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
}