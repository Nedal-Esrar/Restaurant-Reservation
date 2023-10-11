using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
  public ReservationRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId)
  {
    if (!await Context.Customers.AnyAsync(c => c.Id == customerId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", customerId));
    }
    
    return await Context.Reservations
      .Where(r => r.CustomerId == customerId)
      .ToListAsync();
  }

  public async Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync()
  {
    return await Context.ReservationsWithDetails.ToListAsync();
  }
}