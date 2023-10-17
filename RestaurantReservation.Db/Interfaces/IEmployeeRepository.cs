using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
  Task<IEnumerable<Employee>> ListManagersAsync();

  Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync();
}