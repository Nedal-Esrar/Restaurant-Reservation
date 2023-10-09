using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Interfaces;

public interface IEmployeeRepository
{
  Task CreateAsync(Employee employee);

  Task UpdateAsync(Employee employee);

  Task<bool> IsExistAsync(int id);

  Task DeleteAsync(int id);

  Task<IEnumerable<Employee>> ListManagersAsync();

  Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync();
}