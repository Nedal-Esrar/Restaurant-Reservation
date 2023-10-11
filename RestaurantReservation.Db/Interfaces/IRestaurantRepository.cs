using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IRestaurantRepository
{
  Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
}