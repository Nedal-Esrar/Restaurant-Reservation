namespace RestaurantReservation.Api.Models.MenuItems;

public class MenuItemUpdateDto
{
  /// <summary>
  /// The ID of the new restaurant for the menu item.
  /// </summary>
  public int RestaurantId { get; set; }
  
  /// <summary>
  /// The updated name of the menu item.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// The updated description if the menu item.
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// The updated price of the menu item.
  /// </summary>
  public decimal Price { get; set; }
}