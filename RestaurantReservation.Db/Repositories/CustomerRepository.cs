using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository : ICustomerRepository
{
  private readonly RestaurantReservationDbContext _context;

  public CustomerRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Customer customer)
  {
    if (customer is null) throw new ArgumentNullException(nameof(customer));

    await _context.Customers.AddAsync(customer);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Customer customer)
  {
    if (customer is null) throw new ArgumentNullException(nameof(customer));

    if (!await IsExistAsync(customer.CustomerId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", customer.CustomerId));

    _context.Customers.Update(customer);

    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var customer = await _context.Customers.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", id));

    _context.Customers.Remove(customer);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Customers.AnyAsync(c => c.CustomerId == id);
  }

  public async Task<IEnumerable<Customer>> FindCustomersWithPartySizeLargerThanAsync(int minPartySize)
  {
    return await _context.Customers
      .FromSqlInterpolated($"EXEC sp_FindCustomersWithPartySizeLargerThan {minPartySize}")
      .ToListAsync();
  }
}