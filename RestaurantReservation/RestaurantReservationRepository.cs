using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Enums;

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
  
  public async Task CreateEmployeeAsync(Employee employee)
  {
    if (employee is null) throw new ArgumentNullException(nameof(employee));

    await _context.Employees.AddAsync(employee);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateEmployeeAsync(Employee employee)
  {
    if (employee is null) throw new ArgumentNullException(nameof(employee));

    if (!await DoesEmployeeExistAsync(employee.EmployeeId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", employee.EmployeeId));

    _context.Employees.Update(employee);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesEmployeeExistAsync(int id)
  {
    return await _context.Employees.AnyAsync(e => e.EmployeeId == id);
  }

  public async Task DeleteEmployeeAsync(int id)
  {
    var employee = await _context.Employees.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", id));

    _context.Employees.Remove(employee);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateMenuItemAsync(MenuItem menuItem)
  {
    if (menuItem is null) throw new ArgumentNullException(nameof(menuItem));

    await _context.MenuItems.AddAsync(menuItem);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateMenuItemAsync(MenuItem menuItem)
  {
    if (menuItem is null) throw new ArgumentNullException(nameof(menuItem));

    if (!await DoesMenuItemExistAsync(menuItem.ItemId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("MenuItem", menuItem.ItemId));

    _context.MenuItems.Update(menuItem);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesMenuItemExistAsync(int id)
  {
    return await _context.MenuItems.AnyAsync(mi => mi.ItemId == id);
  }

  public async Task DeleteMenuItemAsync(int id)
  {
    var menuItem = await _context.MenuItems.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("MenuItem", id));

    _context.MenuItems.Remove(menuItem);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateOrderAsync(Order order)
  {
    if (order is null) throw new ArgumentNullException(nameof(order));

    await _context.Orders.AddAsync(order);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateOrderAsync(Order order)
  {
    if (order is null) throw new ArgumentNullException(nameof(order));

    if (!await DoesOrderExistAsync(order.OrderId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Order", order.OrderId));

    _context.Orders.Update(order);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesOrderExistAsync(int id)
  {
    return await _context.Orders.AnyAsync(o => o.OrderId == id);
  }

  public async Task DeleteOrderAsync(int id)
  {
    var order = await _context.Orders.FindAsync(id) ??
                throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Order", id));

    _context.Orders.Remove(order);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateOrderItemAsync(OrderItem orderItem)
  {
    if (orderItem is null) throw new ArgumentNullException(nameof(orderItem));

    await _context.OrderItems.AddAsync(orderItem);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateOrderItemAsync(OrderItem orderItem)
  {
    if (orderItem is null) throw new ArgumentNullException(nameof(orderItem));

    if (!await DoesOrderItemExist(orderItem.OrderItemId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("OrderItem", orderItem.OrderItemId));

    _context.OrderItems.Update(orderItem);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesOrderItemExist(int id)
  {
    return await _context.OrderItems.AnyAsync(oi => oi.OrderItemId == id);
  }

  public async Task DeleteOrderItemAsync(int id)
  {
    var orderItem = await _context.OrderItems.FindAsync(id) ??
                    throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("OrderItem", id));

    _context.OrderItems.Remove(orderItem);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateReservationAsync(Reservation reservation)
  {
    if (reservation is null) throw new ArgumentNullException(nameof(reservation));

    await _context.Reservations.AddAsync(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateReservationAsync(Reservation reservation)
  {
    if (reservation is null) throw new ArgumentNullException(nameof(reservation));

    if (!await DoesReservationExist(reservation.ReservationId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", reservation.ReservationId));

    _context.Reservations.Update(reservation);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesReservationExist(int id)
  {
    return await _context.Reservations.AnyAsync(r => r.ReservationId == id);
  }

  public async Task DeleteReservationAsync(int id)
  {
    var reservation = await _context.Reservations.FindAsync(id) ??
                      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Reservation", id));

    _context.Reservations.Remove(reservation);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateRestaurantAsync(Restaurant restaurant)
  {
    if (restaurant is null) throw new ArgumentNullException(nameof(restaurant));

    await _context.Restaurants.AddAsync(restaurant);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateRestaurantAsync(Restaurant restaurant)
  {
    if (restaurant is null) throw new ArgumentNullException(nameof(restaurant));

    if (!await DoesRestaurantExist(restaurant.RestaurantId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", restaurant.RestaurantId));

    _context.Restaurants.Update(restaurant);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesRestaurantExist(int id)
  {
    return await _context.Restaurants.AnyAsync(r => r.RestaurantId == id);
  }

  public async Task DeleteRestaurantAsync(int id)
  {
    var restaurant = await _context.Restaurants.FindAsync(id) ??
                     throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Restaurant", id));

    _context.Restaurants.Remove(restaurant);

    await _context.SaveChangesAsync();
  }
  
  public async Task CreateTableAsync(Table table)
  {
    if (table is null) throw new ArgumentNullException(nameof(table));

    await _context.Tables.AddAsync(table);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateTableAsync(Table table)
  {
    if (table is null) throw new ArgumentNullException(nameof(table));

    if (!await DoesTableExist(table.TableId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Table", table.TableId));

    _context.Tables.Update(table);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> DoesTableExist(int id)
  {
    return await _context.Tables.AnyAsync(r => r.TableId == id);
  }

  public async Task DeleteTableAsync(int id)
  {
    var table = await _context.Tables.FindAsync(id) ??
                throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Table", id));

    _context.Tables.Remove(table);

    await _context.SaveChangesAsync();
  }
  
  public async Task<IEnumerable<Employee>> ListManagersAsync()
  {
    return await _context.Employees
      .Where(e => e.Position == EmployeePosition.Manager)
      .ToListAsync();
  }
}