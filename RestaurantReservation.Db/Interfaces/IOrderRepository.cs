using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
  Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId);

  Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
}