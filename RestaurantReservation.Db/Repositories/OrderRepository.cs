using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository : IOrderRepository
{
  private readonly RestaurantReservationDbContext _context;

  public OrderRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Order order)
  {
    if (order is null) throw new ArgumentNullException(nameof(order));

    await _context.Orders.AddAsync(order);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Order order)
  {
    if (order is null) throw new ArgumentNullException(nameof(order));

    if (!await IsExistAsync(order.OrderId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Order", order.OrderId));

    _context.Orders.Update(order);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Orders.AnyAsync(o => o.OrderId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var order = await _context.Orders.FindAsync(id) ??
                throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Order", id));

    _context.Orders.Remove(order);

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
  {
    if (!await _context.Reservations.AnyAsync(r => r.ReservationId == reservationId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservationId));
    }
    
    return await _context.Orders
      .Where(o => o.ReservationId == reservationId)
      .Include(o => o.OrderItems)
      .ThenInclude(oi => oi.Item)
      .ToListAsync();
  }

  public async Task<double> CalculateAverageOrderAmountAsync(int employeeId)
  {
    if (!await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", employeeId));
    }
    
    var avg = await _context.Orders
      .Where(o => o.EmployeeId == employeeId)
      .AverageAsync(o => o.TotalAmount);

    return avg;
  }
}