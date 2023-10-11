using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
  public CustomerRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<IEnumerable<Customer>> FindCustomersWithPartySizeLargerThanAsync(int minPartySize)
  {
    return await Context.Customers
      .FromSqlInterpolated($"EXEC sp_FindCustomersWithPartySizeLargerThan {minPartySize}")
      .ToListAsync();
  }
}