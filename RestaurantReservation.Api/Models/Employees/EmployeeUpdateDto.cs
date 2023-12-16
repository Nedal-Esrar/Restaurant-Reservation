using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Api.Models.Employees;

public class EmployeeUpdateDto
{
  /// <summary>
  /// The ID of the new restaurant that the employee belongs to.
  /// </summary>
  public int RestaurantId { get; set; }
  
  /// <summary>
  /// The updated first name of the employee.
  /// </summary>
  public string FirstName { get; set; }

  /// <summary>
  /// The updated last name of the employee.
  /// </summary>
  public string LastName { get; set; }
  
  /// <summary>
  /// The updated position of the new employee.
  /// </summary>
  public EmployeePosition Position { get; set; }
}