using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
  Task<IEnumerable<Employee>> ListManagersAsync();

  Task<IEnumerable<EmployeeWithDetails>> GetEmployeesWithDetailsAsync();
}