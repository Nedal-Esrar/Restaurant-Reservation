namespace RestaurantReservation.Api.Models.OrderItems;

public class OrderItemUpdateDto
{
  /// <summary>
  /// The update quantity of the menu item for the order item.
  /// </summary>
  public int Quantity { get; set; }
}