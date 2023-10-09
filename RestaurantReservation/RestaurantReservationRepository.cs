using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation;

public class RestaurantReservationRepository
{
  private readonly RestaurantReservationDbContext _context;
  
  public RestaurantReservationRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }
  
  public async Task CreateCustomerAsync(Customer customer)
  {
    if (customer is null) throw new ArgumentNullException(nameof(customer));

    await _context.Customers.AddAsync(customer);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateCustomerAsync(Customer customer)
  {
    if (customer is null) throw new ArgumentNullException(nameof(customer));

    if (!await DoesCustomerExistAsync(customer.CustomerId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", customer.CustomerId));

    _context.Customers.Update(customer);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesCustomerExistAsync(int id)
  {
    return await _context.Customers.AnyAsync(c => c.CustomerId == id);
  }

  public async Task DeleteCustomerAsync(int id)
  {
    var customer = await _context.Customers.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", id));

    _context.Customers.Remove(customer);

    await _context.SaveChangesAsync();
  }
}