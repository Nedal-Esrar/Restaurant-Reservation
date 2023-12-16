using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Models.OrderItems;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/orders/{orderId}/order-items")]
[ApiController]
[Authorize]
public class OrderItemsController : ControllerBase
{
  private readonly IOrderItemRepository _orderItemRepository;

  private readonly IOrderRepository _orderRepository;

  private readonly IReservationRepository _reservationRepository;

  private readonly IMenuItemRepository _menuItemRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;


  public OrderItemsController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IMapper mapper, IReservationRepository reservationRepository, IMenuItemRepository menuItemRepository)
  {
    _orderItemRepository = orderItemRepository;
    
    _orderRepository = orderRepository;
    
    _mapper = mapper;
    
    _reservationRepository = reservationRepository;
    
    _menuItemRepository = menuItemRepository;
  }

  /// <summary>
  /// Gets order items for an order partitioned into pages.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="404">If the order with the given ID is not found.</response>
  /// <response code="200">Returns the requested page of order items with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItemResponseDto>))]
  public async Task<ActionResult<IEnumerable<OrderItemResponseDto>>> GetOrderItems(int orderId, int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }

    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return NotFound("Order with the given ID is not found.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (orderItems, paginationMetadata) = await _orderItemRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<OrderItemResponseDto>>(orderItems));
  }

  /// <summary>
  /// Returns an order item specified by ID for an order.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="id">The ID of the order item to retrieve.</param>
  /// <response code="404">If the order with the given id or an order item with the given ID for the order is not found.</response>
  /// <response code="200">Returns the requested table.</response>
  [HttpGet("{id}", Name = "GetOrderItem")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemResponseDto))]
  public async Task<ActionResult<OrderItemResponseDto>> GetOrderItem(int orderId, int id)
  {
    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return NotFound("Order with the given ID is not found.");
    }

    var orderItem = await _orderItemRepository.GetByIdAsync(id);

    if (orderItem is null || orderItem.OrderId != orderId)
    {
      return NotFound("Order item with the given ID for the order is not found.");
    }

    return Ok(_mapper.Map<OrderItemResponseDto>(orderItem));
  }

  /// <summary>
  /// Creates a new order item for an order specified by ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="orderItemCreationDto">The data of the new table.</param>
  /// <returns>The newly created table.</returns>
  /// <response code="400">If the creation Data is invalid.</response>
  /// <response code="404">If the order with the given id is not found.</response>
  /// <response code="422">If the menu item is not found or the menu item and the order are not in the same restaurant.</response>
  /// <response code="201">If the table is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderItemResponseDto))]
  public async Task<ActionResult<OrderItemResponseDto>> CreateOrderItem(int orderId, OrderItemCreationDto orderItemCreationDto)
  {
    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return UnprocessableEntity("Order with the given ID is not found.");
    }

    if (!await _menuItemRepository.IsExistAsync(orderItemCreationDto.MenuItemId))
    {
      return UnprocessableEntity("Menu item with the given ID is not found.");
    }

    var areOrderAndMenuItemConsistent = await CheckRestaurantConsistency(orderId, orderItemCreationDto.MenuItemId);

    if (!areOrderAndMenuItemConsistent)
    {
      return UnprocessableEntity("Order and menu item are not in the same restaurant.");
    }
    
    var orderItemToAdd = _mapper.Map<OrderItem>(orderItemCreationDto);
    orderItemToAdd.OrderId = orderId;

    var addedOrderItem = await _orderItemRepository.CreateAsync(orderItemToAdd);

    return CreatedAtRoute(
      "GetOrderItem",
      new { orderId = orderId, id = addedOrderItem.Id },
      _mapper.Map<OrderItemResponseDto>(addedOrderItem)
    );
  }

  private async Task<bool> CheckRestaurantConsistency(int orderId, int menuItemId)
  {
    var order = await _orderRepository.GetByIdAsync(orderId);

    if (order is null) return false;

    var reservation = await _reservationRepository.GetByIdAsync(order.ReservationId.Value);

    if (reservation is null) return false;

    var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);

    if (menuItem is null) return false;

    return reservation.RestaurantId == menuItem.RestaurantId;
  }

  /// <summary>
  /// Updates an existing order item specified by ID for an order specified by ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="id">The ID of the order item to update.</param>
  /// <param name="orderItemUpdateDto">The data for updating the order item.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the order with the given id or an order item with the given ID for the order is not found.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateOrderItem(int orderId, int id, OrderItemUpdateDto orderItemUpdateDto)
  {
    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return NotFound("Order with the given ID is not found.");
    }

    var orderItemEntity = await _orderItemRepository.GetByIdAsync(id);

    if (orderItemEntity is null || orderItemEntity.OrderId != orderId)
    {
      return NotFound("Order item with the given ID for the order is not found.");
    }

    _mapper.Map(orderItemUpdateDto, orderItemEntity);

    await _orderItemRepository.UpdateAsync(orderItemEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing order item specified by ID for an order specified by ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="id">The ID of the table to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the order with the given id or an order item with the given ID for the order is not found.</response>
  /// <response code="204">If successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateOrderItem(int orderId, int id, JsonPatchDocument<OrderItemUpdateDto> patchDocument)
  {
    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return NotFound("Order with the given ID is not found.");
    }
    
    var orderItemEntity = await _orderItemRepository.GetByIdAsync(id);

    if (orderItemEntity is null || orderItemEntity.OrderId != orderId)
    {
      return NotFound("Order item with the given ID for the order is not found.");
    }

    var orderItemToPatch = _mapper.Map<OrderItemUpdateDto>(orderItemEntity);
    
    patchDocument.ApplyTo(orderItemToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(orderItemToPatch))
    {
      return BadRequest(ModelState);
    }

    _mapper.Map(orderItemToPatch, orderItemEntity);

    await _orderItemRepository.UpdateAsync(orderItemEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing order item specified by ID for an order specified by ID.
  /// </summary>
  /// <param name="orderId">The ID of the order.</param>
  /// <param name="id">The ID of the order item to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the table with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteOrderItem(int orderId, int id)
  {
    if (!await _orderRepository.IsExistAsync(orderId))
    {
      return NotFound("Order with the given ID is not found.");
    }

    var orderItem = await _orderItemRepository.GetByIdAsync(id);

    if (orderItem is null || orderItem.OrderId != orderId)
    {
      return NotFound("Order item with the given ID for Order with the given ID is not found.");
    }
    
    await _orderItemRepository.DeleteAsync(id);
    
    return NoContent();
  }
}