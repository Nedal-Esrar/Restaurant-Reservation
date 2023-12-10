using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IRestaurantRepository : IRepository<Restaurant>
{
  Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
}