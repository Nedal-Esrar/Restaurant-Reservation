using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
  public EmployeeRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public async Task<IEnumerable<Employee>> ListManagersAsync()
  {
    return await Context.Employees
      .Where(e => e.Position == EmployeePosition.Manager)
      .ToListAsync();
  }

  public async Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync()
  {
    return await Context.EmployeesWithDetails.ToListAsync();
  }
  
  public override async Task DeleteAsync(int id)
  {
    if (!await IsExistAsync(id))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Employee", id));
    }

    var employee = await Context.Employees
      .Include(e => e.Orders)
      .FirstAsync(e => e.Id == id);
    
    foreach (var employeeOrder in employee.Orders)
    {
      employeeOrder.EmployeeId = null;
    }

    Context.Employees.Remove(employee);

    await Context.SaveChangesAsync();
  }
}