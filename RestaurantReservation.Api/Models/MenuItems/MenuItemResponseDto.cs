namespace RestaurantReservation.Api.Models.MenuItems;

public class MenuItemResponseDto
{
  public int Id { get; set; }
  
  public int? RestaurantId { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public decimal Price { get; set; }
}