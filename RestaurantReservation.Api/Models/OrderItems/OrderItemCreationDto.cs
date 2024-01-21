namespace RestaurantReservation.Api.Models.OrderItems;

public class OrderItemCreationDto
{
  /// <summary>
  /// The ID of the menu item for the new order item.
  /// </summary>
  public int MenuItemId { get; set; }
  
  /// <summary>
  /// The quantity of the menu item in the new order item.
  /// </summary>
  public int Quantity { get; set; }
}