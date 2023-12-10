using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Api.Models.Employees;

public class EmployeeResponseDto
{
  public int Id { get; set; }
  
  public int RestaurantId { get; set; }
  
  public string FirstName { get; set; }

  public string LastName { get; set; }
  
  public EmployeePosition Position { get; set; }
}