using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Models.Reservations;
using RestaurantReservation.Api.Models.Restaurants;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/restaurants")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class RestaurantsController : ControllerBase
{
  private readonly IRestaurantRepository _restaurantRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;

  public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
  {
    _restaurantRepository = restaurantRepository;

    _mapper = mapper;
  }

  /// <summary>
  /// Gets restaurants partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of restaurants with pagination metadata in the headers.</response>
  /// <response code="403">When the user is not an admin.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantResponseDto>))]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<ActionResult<IEnumerable<RestaurantResponseDto>>> GetRestaurants(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (restaurants, paginationMetadata) = await _restaurantRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<RestaurantResponseDto>>(restaurants));
  }

  /// <summary>
  /// Returns a restaurant specified by ID.
  /// </summary>
  /// <param name="id">The ID of the restaurant to retrieve.</param>
  /// <response code="404">If the restaurant with the given id is not found.</response>
  /// <response code="200">Returns the requested restaurant.</response>
  [HttpGet("{id}", Name = "GetRestaurant")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantResponseDto))]
  public async Task<ActionResult<RestaurantResponseDto>> GetRestaurant(int id)
  {
    var restaurant = await _restaurantRepository.GetByIdAsync(id);

    if (restaurant is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<RestaurantResponseDto>(restaurant));
  }

  /// <summary>
  /// Creates a new restaurant.
  /// </summary>
  /// <param name="restaurantCreationDto">The data of the new restaurant.</param>
  /// <returns>The newly created restaurant.</returns>
  /// <response code="400">If the restaurant data is invalid.</response>
  /// <response code="201">If the restaurant is created successfully.</response>
  /// <response code="403">When the user is not an admin.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestaurantResponseDto))]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<ActionResult<RestaurantResponseDto>> CreateRestaurant(RestaurantCreationDto restaurantCreationDto)
  {
    var restaurantToAdd = _mapper.Map<Restaurant>(restaurantCreationDto);

    var addedRestaurant = await _restaurantRepository.CreateAsync(restaurantToAdd);

    return CreatedAtRoute(
      "GetRestaurant",
      new { id = addedRestaurant.Id },
      _mapper.Map<ReservationResponseDto>(addedRestaurant)
    );
  }

  /// <summary>
  /// Updates an existing restaurant specified by ID.
  /// </summary>
  /// <param name="id">The ID of the restaurant to update.</param>
  /// <param name="restaurantUpdateDto">The data for updating the restaurant.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the restaurant with the specified ID is not found.</response>
  /// <response code="204">If successful.</response>
  /// <response code="403">When the user is not an admin.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto)
  {
    var restaurantEntity = await _restaurantRepository.GetByIdAsync(id);

    if (restaurantEntity is null)
    {
      return NotFound();
    }

    _mapper.Map(restaurantUpdateDto, restaurantEntity);

    await _restaurantRepository.UpdateAsync(restaurantEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing restaurant specified by ID.
  /// </summary>
  /// <param name="id">The ID of the restaurant to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the restaurant with the specified ID is not found.</response>
  /// <response code="204">If the update is successful.</response>
  /// <response code="403">When the user is not an admin.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> PartiallyUpdateRestaurant(int id, JsonPatchDocument<RestaurantUpdateDto> patchDocument)
  {
    var restaurantEntity = await _restaurantRepository.GetByIdAsync(id);

    if (restaurantEntity is null)
    {
      return NotFound();
    }

    var restaurantToPatch = _mapper.Map<RestaurantUpdateDto>(restaurantEntity);
    
    patchDocument.ApplyTo(restaurantToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(restaurantToPatch))
    {
      return BadRequest(ModelState);
    }

    _mapper.Map(restaurantToPatch, restaurantEntity);

    await _restaurantRepository.UpdateAsync(restaurantEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing restaurant specified by ID.
  /// </summary>
  /// <param name="id">The ID of the restaurant to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the restaurant with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  /// <response code="403">When the user is not an admin.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> DeleteRestaurant(int id)
  {
    try
    {
      await _restaurantRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}