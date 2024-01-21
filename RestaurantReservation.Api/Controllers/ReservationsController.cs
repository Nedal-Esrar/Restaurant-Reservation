using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Models.MenuItems;
using RestaurantReservation.Api.Models.Orders;
using RestaurantReservation.Api.Models.Reservations;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationsController : ControllerBase
{
  private readonly IReservationRepository _reservationRepository;

  private readonly ICustomerRepository _customerRepository;

  private readonly IRestaurantRepository _restaurantRepository;
  
  private readonly ITableRepository _tableRepository;

  private readonly IOrderRepository _orderRepository;

  private readonly IMenuItemRepository _menuItemRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;

  public ReservationsController(IReservationRepository reservationRepository, IMapper mapper, ICustomerRepository customerRepository, IRestaurantRepository restaurantRepository, ITableRepository tableRepository, IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
  {
    _reservationRepository = reservationRepository;
    
    _mapper = mapper;
    
    _customerRepository = customerRepository;
    
    _restaurantRepository = restaurantRepository;
    
    _tableRepository = tableRepository;
    
    _orderRepository = orderRepository;
    
    _menuItemRepository = menuItemRepository;
  }

  /// <summary>
  /// Gets reservations partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of employees with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponseDto>))]
  public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservations(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (reservations, paginationMetadata) = await _reservationRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<ReservationResponseDto>>(reservations));
  }

  /// <summary>
  /// Returns a reservation specified by ID.
  /// </summary>
  /// <param name="id">The ID of the reservation to retrieve.</param>
  /// <response code="404">If the reservation with the given id is not found.</response>
  /// <response code="200">Returns the requested reservation.</response>
  [HttpGet("{id}", Name = "GetReservation")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationResponseDto))]
  public async Task<ActionResult<ReservationResponseDto>> GetReservation(int id)
  {
    var reservation = await _reservationRepository.GetByIdAsync(id);

    if (reservation is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<ReservationResponseDto>(reservation));
  }

  /// <summary>
  /// Gets reservation for a customer specified by ID partitioned into pages.
  /// </summary>
  /// <param name="customerId">The ID of the customer to get reservations for.</param>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="404">If the reservation with the given id is not found.</response>
  /// <response code="200">Returns the requested page of reservations with pagination metadata in the headers.</response>
  [HttpGet("customer/{customerId}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReservationResponseDto>))]
  public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservationsForCustomer(int customerId, int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }

    if (!await _customerRepository.IsExistAsync(customerId))
    {
      return NotFound("Customer with the given ID is not found.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (reservations, paginationMetadata) = await _reservationRepository.GetAllAsync(r => r.CustomerId == customerId, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<ReservationResponseDto>>(reservations));
  }

  /// <summary>
  /// Gets orders for a reservation specified by ID.
  /// </summary>
  /// <param name="id">The ID of the reservation.</param>
  /// <response code="404">If the reservation with the given id is not found.</response>
  /// <response code="200">Returns the requested orders.</response>
  [HttpGet("{id}/orders")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderWithMenuItemsResponseDto>))]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<OrderWithMenuItemsResponseDto>>> GetOrdersForReservation(int id)
  {
    try
    {
      var orders = await _orderRepository.ListOrdersAndMenuItemsAsync(id);

      return Ok(_mapper.Map<IEnumerable<OrderWithMenuItemsResponseDto>>(orders));
    }
    catch (NotFoundException)
    {
      return NotFound("Reservation with the given ID is not found.");
    }
  }

  /// <summary>
  /// Retrieves ordered menu items for a reservation. 
  /// </summary>
  /// <param name="id">The ID of the reservation.</param>
  /// <response code="404">If the reservation with the given id is not found.</response>
  /// <response code="200">Returns the requested menu items.</response>
  [HttpGet("{id}/menu-items")]
  public async Task<ActionResult<IEnumerable<MenuItemResponseDto>>> GetOrderedMenuItems(int id)
  {
    try
    {
      var menuItems = await _menuItemRepository.ListOrderedMenuItemsAsync(id);

      return Ok(_mapper.Map<IEnumerable<MenuItemResponseDto>>(menuItems));
    }
    catch (NotFoundException)
    {
      return NotFound("Reservation with the given ID is not found.");
    }
  }

  /// <summary>
  /// Creates a new reservation.
  /// </summary>
  /// <param name="reservationCreationDto">The data of the new reservation.</param>
  /// <returns>The newly created reservation.</returns>
  /// <response code="400">If the creation data is invalid.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="201">If the reservation is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReservationResponseDto))]
  public async Task<ActionResult<ReservationResponseDto>> CreateReservation(
    ReservationCreationDto reservationCreationDto)
  {
    var integrityErrors = await CheckForeignKeysIntegrity(
      reservationCreationDto.CustomerId,
      reservationCreationDto.RestaurantId,
      reservationCreationDto.TableId
    );  

    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }
    
    var reservationToAdd = _mapper.Map<Reservation>(reservationCreationDto);

    var addedReservation = await _reservationRepository.CreateAsync(reservationToAdd);

    return CreatedAtRoute(
      "GetReservation",
      new { id = addedReservation.Id },
      _mapper.Map<ReservationResponseDto>(addedReservation)
    );
  }

  private async Task<List<string>> CheckForeignKeysIntegrity(int customerId, int restaurantId, int tableId)
  {
    var integrityErrors = new List<string>();
    
    if (!await _customerRepository.IsExistAsync(customerId))
    {
      integrityErrors.Add("Customer with the given id is not found.");
    }
    
    var table = await _tableRepository.GetByIdAsync(tableId);

    if (table is null)
    {
      integrityErrors.Add("Table with the given id is not found.");
    }

    var doesRestaurantExist = await _restaurantRepository.IsExistAsync(restaurantId);
    
    if (!doesRestaurantExist)
    {
      integrityErrors.Add("Restaurant with the given id is not found.");
    }

    if (doesRestaurantExist && table is not null && table.RestaurantId != restaurantId)
    {
      integrityErrors.Add("Table with the given ID is not in the Restaurant with the given ID.");
    }

    return integrityErrors;
  }

  /// <summary>
  /// Updates an existing reservation specified by ID.
  /// </summary>
  /// <param name="id">The ID of the reservation to update.</param>
  /// <param name="reservationUpdateDto">The data for updating the reservation.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the reservation with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateReservation(int id, ReservationUpdateDto reservationUpdateDto)
  {
    var reservationEntity = await _reservationRepository.GetByIdAsync(id);

    if (reservationEntity is null)
    {
      return NotFound();
    }
    
    var integrityErrors = await CheckForeignKeysIntegrity(
      reservationUpdateDto.CustomerId,
      reservationUpdateDto.RestaurantId,
      reservationUpdateDto.TableId
    );  

    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }

    _mapper.Map(reservationUpdateDto, reservationEntity);

    await _reservationRepository.UpdateAsync(reservationEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing reservation specified by ID.
  /// </summary>
  /// <param name="id">The ID of the reservation to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the restaurant with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateReservation(int id, JsonPatchDocument<ReservationUpdateDto> patchDocument)
  {
    var reservationEntity = await _restaurantRepository.GetByIdAsync(id);

    if (reservationEntity is null)
    {
      return NotFound();
    }

    var reservationToPatch = _mapper.Map<ReservationUpdateDto>(reservationEntity);
    
    patchDocument.ApplyTo(reservationToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(reservationToPatch))
    {
      return BadRequest(ModelState);
    }
    
    var integrityErrors = await CheckForeignKeysIntegrity(
      reservationToPatch.CustomerId,
      reservationToPatch.RestaurantId,
      reservationToPatch.TableId
    );  

    if (integrityErrors.Any())
    {
      return UnprocessableEntity(string.Join(Environment.NewLine, integrityErrors));
    }

    _mapper.Map(reservationToPatch, reservationEntity);

    await _restaurantRepository.UpdateAsync(reservationEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing reservation specified by ID.
  /// </summary>
  /// <param name="id">The ID of the reservation to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the reservation with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteReservation(int id)
  {
    try
    {
      await _reservationRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}