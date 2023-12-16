using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Api.Models.Employees;

public class EmployeeCreationDto
{
  /// <summary>
  /// The ID of the restaurant that the new employee belongs to.
  /// </summary>
  public int RestaurantId { get; set; }
  
  /// <summary>
  /// The first name of the new employee.
  /// </summary>
  public string FirstName { get; set; }

  /// <summary>
  /// The last name of the new employee.
  /// </summary>
  public string LastName { get; set; }
  
  /// <summary>
  /// The position of the new employee.
  /// </summary>
  public EmployeePosition Position { get; set; }
}