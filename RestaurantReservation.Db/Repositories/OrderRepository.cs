using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Utilities;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
  public OrderRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
  {
    if (!await Context.Reservations.AnyAsync(r => r.Id == reservationId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservationId));
    }
    
    return await Context.Orders
      .Where(o => o.ReservationId == reservationId)
      .Include(o => o.OrderItems)
      .ThenInclude(oi => oi.Item)
      .ToListAsync();
  }

  public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
  {
    if (!await Context.Employees.AnyAsync(e => e.Id == employeeId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", employeeId));
    }
    
    var avg = await Context.Orders
      .Where(o => o.EmployeeId == employeeId)
      .AverageAsync(o => o.TotalAmount);

    return avg;
  }
}