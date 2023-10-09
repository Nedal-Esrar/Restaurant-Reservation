using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository : IReservationRepository
{
  private readonly RestaurantReservationDbContext _context;

  public ReservationRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Reservation reservation)
  {
    if (reservation is null) throw new ArgumentNullException(nameof(reservation));

    await _context.Reservations.AddAsync(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Reservation reservation)
  {
    if (reservation is null) throw new ArgumentNullException(nameof(reservation));

    if (!await IsExistAsync(reservation.ReservationId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservation.ReservationId));

    _context.Reservations.Update(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Reservations.AnyAsync(r => r.ReservationId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var reservation = await _context.Reservations.FindAsync(id) ??
                      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", id));

    _context.Reservations.Remove(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId)
  {
    if (!await _context.Customers.AnyAsync(c => c.CustomerId == customerId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Customer", customerId));
    }
    
    return await _context.Reservations
      .Where(r => r.CustomerId == customerId)
      .ToListAsync();
  }

  public async Task<IEnumerable<ReservationWithDetails>> GetReservationsWithDetailsAsync()
  {
    return await _context.ReservationsWithDetails.ToListAsync();
  }
}