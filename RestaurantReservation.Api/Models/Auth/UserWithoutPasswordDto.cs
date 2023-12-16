namespace RestaurantReservation.Api.Models.Auth;

public class UserWithoutPasswordDto
{
  public string Username { get; set; }
  
  public List<RoleDto> Roles { get; set; }
}