using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IRestaurantRepository
{
  Task CreateAsync(Restaurant restaurant);

  Task UpdateAsync(Restaurant restaurant);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);
  
  Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
}