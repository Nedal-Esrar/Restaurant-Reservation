using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Utilities;

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
  
  public override async Task DeleteAsync(int id)
  {
    if (!await IsExistAsync(id))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", id));
    }

    var reservation = await Context.Reservations
      .Include(r => r.Orders)
      .FirstAsync(c => c.Id == id);
    
    foreach (var reservationOrder in reservation.Orders)
    {
      reservationOrder.ReservationId = null;
    }
    
    Context.Reservations.Remove(reservation);

    await Context.SaveChangesAsync();
  }
}