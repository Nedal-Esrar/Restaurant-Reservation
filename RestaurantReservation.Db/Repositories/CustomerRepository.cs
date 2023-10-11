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
  
  public override async Task DeleteAsync(int id)
  {
    if (!await IsExistAsync(id))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", id));
    }

    var customer = await Context.Customers
      .Include(c => c.Reservations)
      .FirstAsync(c => c.Id == id);
    
    foreach (var customerReservation in customer.Reservations)
    {
      customerReservation.CustomerId = null;
    }
    
    Context.Customers.Remove(customer);

    await Context.SaveChangesAsync();
  }
}