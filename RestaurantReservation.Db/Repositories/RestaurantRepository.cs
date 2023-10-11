using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
{
  public RestaurantRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
  {
    if (!await IsExistAsync(restaurantId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", restaurantId));
    }

    var revenue = await Context.Restaurants
      .Where(r => r.Id == restaurantId)
      .Select(r => Context.CalculateRestaurantRevenue(r.Id))
      .FirstOrDefaultAsync();

    return revenue;
  }
}