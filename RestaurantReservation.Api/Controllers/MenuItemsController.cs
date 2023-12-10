using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Models.MenuItems;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/menu-items")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class MenuItemsController : ControllerBase
{
  private readonly IMenuItemRepository _menuItemRepository;

  private readonly IRestaurantRepository _restaurantRepository;

  private readonly IMapper _mapper;
  
  private const int MaxPageSize = 20;

  public MenuItemsController(IMenuItemRepository menuItemRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
  {
    _menuItemRepository = menuItemRepository;
    
    _restaurantRepository = restaurantRepository;
    
    _mapper = mapper;
  }

  /// <summary>
  /// Gets menu items partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of menu items with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MenuItemResponseDto>))]
  public async Task<ActionResult<IEnumerable<MenuItemResponseDto>>> GetMenuItems(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (menuItems, paginationMetadata) = await _menuItemRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<MenuItemResponseDto>>(menuItems));
  }

  /// <summary>
  /// Returns a menu item specified by ID.
  /// </summary>
  /// <param name="id">The ID of the menu item to retrieve.</param>
  /// <response code="404">If the menu item with the given id is not found.</response>
  /// <response code="200">Returns the requested order.</response>
  [HttpGet("{id}", Name = "GetMenuItem")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuItemResponseDto))]
  public async Task<ActionResult<MenuItemResponseDto>> GetMenuItem(int id)
  {
    var menuItem = await _menuItemRepository.GetByIdAsync(id);

    if (menuItem is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<MenuItemResponseDto>(menuItem));
  }
  
  /// <summary>
  /// Returns menu items in a restaurant specified by ID partitioned into pages.
  /// </summary>
  /// <param name="restaurantId">The ID of the restaurant.</param>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="404">If the restaurant with the specified id is not found.</response>
  /// <response code="200">Returns the requested menu items.</response>
  [HttpGet("restaurant/{restaurantId}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MenuItemResponseDto>))]
  public async Task<ActionResult<IEnumerable<MenuItemResponseDto>>> GetMenuItemsForRestaurant(int restaurantId, int pageNumber, int pageSize)
  {
    if (!await _restaurantRepository.IsExistAsync(restaurantId))
    {
      return NotFound("Restaurant with the given ID is not found.");
    }

    var (restaurantMenuItems, paginationMetadata) = await _menuItemRepository.GetAllAsync(mi => mi.RestaurantId == restaurantId, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<MenuItemResponseDto>>(restaurantMenuItems));
  }

  /// <summary>
  /// Creates a new menu item.
  /// </summary>
  /// <param name="menuItemCreationDto">The data of the new menu item.</param>
  /// <returns>The newly created menu item.</returns>
  /// <response code="400">If the creation data is invalid.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="201">If the menu item is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MenuItemResponseDto))]
  public async Task<ActionResult<MenuItemResponseDto>> CreateMenuItem(MenuItemCreationDto menuItemCreationDto)
  {
    if (!await _restaurantRepository.IsExistAsync(menuItemCreationDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }
    
    var menuItemToAdd = _mapper.Map<MenuItem>(menuItemCreationDto);

    var addedMenuItem = await _menuItemRepository.CreateAsync(menuItemToAdd);

    return CreatedAtRoute(
      "GetMenuItem",
      new { id = addedMenuItem.Id },
      _mapper.Map<MenuItemResponseDto>(addedMenuItem)
    );
  }

  /// <summary>
  /// Updates an existing menu item specified by ID.
  /// </summary>
  /// <param name="id">The ID of the menu item to update.</param>
  /// <param name="menuItemUpdateDto">The data for updating the menu item.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the menu item with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateMenuItem(int id, MenuItemUpdateDto menuItemUpdateDto)
  {
    var menuItemEntity = await _menuItemRepository.GetByIdAsync(id);

    if (menuItemEntity is null)
    {
      return NotFound();
    }
    
    if (!await _restaurantRepository.IsExistAsync(menuItemUpdateDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(menuItemUpdateDto, menuItemEntity);

    await _menuItemRepository.UpdateAsync(menuItemEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing menu item specified by ID.
  /// </summary>
  /// <param name="id">The ID of the menu item to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the menu item with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateMenuItem(int id, JsonPatchDocument<MenuItemUpdateDto> patchDocument)
  {
    var menuItemEntity = await _menuItemRepository.GetByIdAsync(id);

    if (menuItemEntity is null)
    {
      return NotFound();
    }

    var menuItemToPatch = _mapper.Map<MenuItemUpdateDto>(menuItemEntity);
    
    patchDocument.ApplyTo(menuItemToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(menuItemToPatch))
    {
      return BadRequest(ModelState);
    }
    
    if (!await _restaurantRepository.IsExistAsync(menuItemToPatch.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(menuItemToPatch, menuItemEntity);

    await _menuItemRepository.UpdateAsync(menuItemEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing menu item specified by ID.
  /// </summary>
  /// <param name="id">The Id of the menu item to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the menu item with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteMenuItem(int id)
  {
    try
    {
      await _menuItemRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}