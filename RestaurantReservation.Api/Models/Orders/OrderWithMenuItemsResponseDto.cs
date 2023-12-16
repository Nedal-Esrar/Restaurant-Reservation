using RestaurantReservation.Api.Models.MenuItems;

namespace RestaurantReservation.Api.Models.Orders;

public class OrderWithMenuItemsResponseDto
{
  public int Id { get; set; }
  
  public int? ReservationId { get; set; }
  
  public int? EmployeeId { get; set; }

  public DateTime OrderDate { get; set; }

  public decimal TotalAmount { get; set; }

  public List<MenuItemResponseDto> MenuItems { get; set; }
}