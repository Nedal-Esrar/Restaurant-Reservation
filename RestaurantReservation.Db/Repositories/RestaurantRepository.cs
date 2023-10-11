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
  
  public override async Task DeleteAsync(int id)
  {
    if (!await IsExistAsync(id))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", id));
    }
  
    var restaurant = await Context.Restaurants
      .Include(rs => rs.Reservations)
      .Include(rs => rs.Employees)
      .Include(rs => rs.Tables)
      .FirstAsync(c => c.Id == id);
    
    foreach (var restaurantReservation in restaurant.Reservations)
    {
      restaurantReservation.RestaurantId = null;
    }
    
    foreach (var restaurantEmployee in restaurant.Employees)
    {
      restaurantEmployee.RestaurantId = null;
    }
    
    foreach (var restaurantTable in restaurant.Tables)
    {
      restaurantTable.RestaurantId = null;
    }
    
    Context.Restaurants.Remove(restaurant);
  
    await Context.SaveChangesAsync();
  }
}