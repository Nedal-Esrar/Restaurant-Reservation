using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
  private readonly RestaurantReservationDbContext _context;

  public RestaurantRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Restaurant restaurant)
  {
    if (restaurant is null) throw new ArgumentNullException(nameof(restaurant));

    await _context.Restaurants.AddAsync(restaurant);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Restaurant restaurant)
  {
    if (restaurant is null) throw new ArgumentNullException(nameof(restaurant));

    if (!await IsExistAsync(restaurant.RestaurantId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", restaurant.RestaurantId));

    _context.Restaurants.Update(restaurant);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Restaurants.AnyAsync(r => r.RestaurantId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var restaurant = await _context.Restaurants.FindAsync(id) ??
                     throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", id));

    _context.Restaurants.Remove(restaurant);

    await _context.SaveChangesAsync();
  }

  public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
  {
    if (!await IsExistAsync(restaurantId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", restaurantId));
    }

    var revenue = await _context.Restaurants
      .Where(r => r.RestaurantId == restaurantId)
      .Select(r => _context.CalculateRestaurantRevenue(r.RestaurantId))
      .FirstOrDefaultAsync();

    return revenue;
  }
}