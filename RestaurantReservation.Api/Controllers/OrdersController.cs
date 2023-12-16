using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Models.Orders;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/orders")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
  private readonly IOrderRepository _orderRepository;

  private readonly IReservationRepository _reservationRepository;

  private readonly IEmployeeRepository _employeeRepository;

  private readonly IMapper _mapper;
  
  private const int MaxPageSize = 20;

  public OrdersController(IMapper mapper, IOrderRepository orderRepository, IReservationRepository reservationRepository, IEmployeeRepository employeeRepository)
  {
    _mapper = mapper;
    
    _orderRepository = orderRepository;
    _reservationRepository = reservationRepository;
    _employeeRepository = employeeRepository;
  }

  /// <summary>
  /// Gets orders partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of orders with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponseDto>))]
  public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (orders, paginationMetadata) = await _orderRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<OrderResponseDto>>(orders));
  }

  /// <summary>
  /// Returns an order specified by ID.
  /// </summary>
  /// <param name="id">The ID of the order to retrieve.</param>
  /// <response code="404">If the order with the given id is not found.</response>
  /// <response code="200">Returns the requested order.</response>
  [HttpGet("{id}", Name = "GetOrder")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDto))]
  public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
  {
    var table = await _orderRepository.GetByIdAsync(id);

    if (table is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<OrderResponseDto>(table));
  }
  
  /// <summary>
  /// Returns orders for a reservation specified by ID partitioned into pages.
  /// </summary>
  /// <param name="reservationId">The ID of the reservation.</param>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="404">If the reservation with the specified id is not found.</response>
  /// <response code="200">Returns the requested orders.</response>
  [HttpGet("reservation/{reservationId}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderResponseDto>))]
  public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersForReservation(int reservationId, int pageNumber, int pageSize)
  {
    if (!await _reservationRepository.IsExistAsync(reservationId))
    {
      return NotFound("Reservation with the given ID is not found.");
    }

    var (reservationOrders, paginationMetadata) = await _orderRepository.GetAllAsync(o => o.ReservationId == reservationId, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<OrderResponseDto>>(reservationOrders));
  }

  /// <summary>
  /// Creates a new order.
  /// </summary>
  /// <param name="orderCreationDto">The data of the new order.</param>
  /// <returns>The newly created order.</returns>
  /// <response code="400">If the creation data is invalid.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="201">If the order is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDto))]
  public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderCreationDto orderCreationDto)
  {
    var integrityErrors = await CheckForeignKeysIntegrity(
      orderCreationDto.ReservationId,
      orderCreationDto.EmployeeId
    );  
    
    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }
    
    var orderToAdd = _mapper.Map<Order>(orderCreationDto);

    var addedOrder = await _orderRepository.CreateAsync(orderToAdd);

    return CreatedAtRoute(
      "GetOrder",
      new { id = addedOrder.Id },
      _mapper.Map<OrderResponseDto>(addedOrder)
    );
  }

  private async Task<List<string>> CheckForeignKeysIntegrity(int reservationId, int employeeId)
  {
    var integrityErrors = new List<string>();

    var reservation = await _reservationRepository.GetByIdAsync(reservationId);

    if (reservation is null)
    {
      integrityErrors.Add("Reservation with the given ID is not found.");
    }

    var employee = await _employeeRepository.GetByIdAsync(employeeId);

    if (employee is null)
    {
      integrityErrors.Add("Employee with the given ID is not found.");
    }

    if (reservation is not null && employee is not null 
                                && reservation.RestaurantId != employee.RestaurantId)
    {
      integrityErrors.Add("Employee with the given ID is not in the same restaurant as the reservation with the given ID");
    }

    return integrityErrors;
  }

  /// <summary>
  /// Updates an existing order specified by ID.
  /// </summary>
  /// <param name="id">The ID of the order to update.</param>
  /// <param name="orderUpdateDto">The data for updating the order.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the order with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
  {
    var orderEntity = await _orderRepository.GetByIdAsync(id);

    if (orderEntity is null)
    {
      return NotFound();
    }
    
    var integrityErrors = await CheckForeignKeysIntegrity(
      orderUpdateDto.ReservationId,
      orderUpdateDto.EmployeeId
    );  
    
    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }

    _mapper.Map(orderUpdateDto, orderEntity);

    await _orderRepository.UpdateAsync(orderEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing order specified by ID.
  /// </summary>
  /// <param name="id">The ID of the order to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the order with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateOrder(int id, JsonPatchDocument<OrderUpdateDto> patchDocument)
  {
    var orderEntity = await _orderRepository.GetByIdAsync(id);

    if (orderEntity is null)
    {
      return NotFound();
    }

    var orderToPatch = _mapper.Map<OrderUpdateDto>(orderEntity);
    
    patchDocument.ApplyTo(orderToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(orderToPatch))
    {
      return BadRequest(ModelState);
    }
    
    var integrityErrors = await CheckForeignKeysIntegrity(
      orderToPatch.ReservationId,
      orderToPatch.EmployeeId
    );  
    
    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }

    _mapper.Map(orderToPatch, orderEntity);

    await _orderRepository.UpdateAsync(orderEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing order specified by ID.
  /// </summary>
  /// <param name="id">The Id of the order to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the order with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  /// <response code="403">The user is not an admin.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> DeleteOrder(int id)
  {
    try
    {
      await _orderRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}