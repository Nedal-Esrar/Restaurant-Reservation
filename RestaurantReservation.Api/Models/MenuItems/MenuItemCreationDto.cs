namespace RestaurantReservation.Api.Models.MenuItems;

public class MenuItemCreationDto
{
  /// <summary>
  /// The ID of the restaurant for the new menu item.
  /// </summary>
  public int RestaurantId { get; set; }
  
  /// <summary>
  /// The name of the new menu item.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// The description if the new menu item.
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// The price of the new menu item.
  /// </summary>
  public decimal Price { get; set; }
}