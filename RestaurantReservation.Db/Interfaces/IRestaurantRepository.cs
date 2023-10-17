using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IRestaurantRepository : IRepository<Restaurant>
{
  Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
}