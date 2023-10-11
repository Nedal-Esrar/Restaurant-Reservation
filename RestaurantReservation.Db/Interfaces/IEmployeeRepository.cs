using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IEmployeeRepository
{
  Task<IEnumerable<Employee>> ListManagersAsync();

  Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync();
}