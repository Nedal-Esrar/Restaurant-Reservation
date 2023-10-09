using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
  private readonly RestaurantReservationDbContext _context;

  public EmployeeRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Employee employee)
  {
    if (employee is null) throw new ArgumentNullException(nameof(employee));

    await _context.Employees.AddAsync(employee);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Employee employee)
  {
    if (employee is null) throw new ArgumentNullException(nameof(employee));

    if (!await IsExistAsync(employee.EmployeeId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", employee.EmployeeId));

    _context.Employees.Update(employee);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Employees.AnyAsync(e => e.EmployeeId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var employee = await _context.Employees.FindAsync(id) ??
                   throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", id));

    _context.Employees.Remove(employee);

    await _context.SaveChangesAsync();
  }

  public async Task<IEnumerable<Employee>> ListManagersAsync()
  {
    return await _context.Employees
      .Where(e => e.Position == EmployeePosition.Manager)
      .ToListAsync();
  }

  public async Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync()
  {
    return await _context.EmployeesWithDetails.ToListAsync();
  }
}