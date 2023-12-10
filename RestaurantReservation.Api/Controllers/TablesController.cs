using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Models.Tables;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/tables")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class TablesController : ControllerBase
{
  private readonly ITableRepository _tableRepository;

  private readonly IRestaurantRepository _restaurantRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;

  public TablesController(ITableRepository tableRepository, IMapper mapper, IRestaurantRepository restaurantRepository)
  {
    _tableRepository = tableRepository;

    _mapper = mapper;
    
    _restaurantRepository = restaurantRepository;
  }

  /// <summary>
  /// Gets tables partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of tables with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TableResponseDto>))]
  public async Task<ActionResult<IEnumerable<TableResponseDto>>> GetTables(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (tables, paginationMetadata) = await _tableRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<TableResponseDto>>(tables));
  }

  /// <summary>
  /// Returns a table specified by ID.
  /// </summary>
  /// <param name="id">The ID of the table to retrieve.</param>
  /// <response code="404">If the table with the given id is not found.</response>
  /// <response code="200">Returns the requested table.</response>
  [HttpGet("{id}", Name = "GetTable")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableResponseDto))]
  public async Task<ActionResult<TableResponseDto>> GetTable(int id)
  {
    var table = await _tableRepository.GetByIdAsync(id);

    if (table is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<TableResponseDto>(table));
  }
  
  /// <summary>
  /// Returns tables in a restaurant specified by ID partitioned into pages.
  /// </summary>
  /// <param name="restaurantId">The ID of the restaurant.</param>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="404">If the restaurant with the specified id is not found.</response>
  /// <response code="200">Returns the requested tables.</response>
  [HttpGet("restaurant/{restaurantId}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TableResponseDto>))]
  public async Task<ActionResult<IEnumerable<TableResponseDto>>> GetTablesForRestaurant(int restaurantId, int pageNumber, int pageSize)
  {
    if (!await _tableRepository.IsExistAsync(restaurantId))
    {
      return NotFound("Restaurant with the given ID is not found.");
    }

    var (restaurantTables, paginationMetadata) = await _tableRepository.GetAllAsync(t => t.RestaurantId == restaurantId, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<TableResponseDto>>(restaurantTables));
  }

  /// <summary>
  /// Creates a new table.
  /// </summary>
  /// <param name="tableCreationDto">The data of the new table.</param>
  /// <returns>The newly created table.</returns>
  /// <response code="400">If the creation Data is invalid.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="201">If the table is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TableResponseDto))]
  public async Task<ActionResult<TableResponseDto>> CreateTable(TableCreationDto tableCreationDto)
  {
    if (!await _restaurantRepository.IsExistAsync(tableCreationDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }
    
    var tableToAdd = _mapper.Map<Table>(tableCreationDto);

    var addedTable = await _tableRepository.CreateAsync(tableToAdd);

    return CreatedAtRoute(
      "GetTable",
      new { id = addedTable.Id },
      _mapper.Map<TableResponseDto>(addedTable)
    );
  }

  /// <summary>
  /// Updates an existing table specified by ID.
  /// </summary>
  /// <param name="id">The ID of the table to update.</param>
  /// <param name="tableUpdateDto">The data for updating the table.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the table with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateTable(int id, TableUpdateDto tableUpdateDto)
  {
    var tableEntity = await _tableRepository.GetByIdAsync(id);

    if (tableEntity is null)
    {
      return NotFound();
    }
    
    if (!await _restaurantRepository.IsExistAsync(tableUpdateDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(tableUpdateDto, tableEntity);

    await _tableRepository.UpdateAsync(tableEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing table specified by ID.
  /// </summary>
  /// <param name="id">The ID of the table to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the table with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateTable(int id, JsonPatchDocument<TableUpdateDto> patchDocument)
  {
    var tableEntity = await _tableRepository.GetByIdAsync(id);

    if (tableEntity is null)
    {
      return NotFound();
    }

    var tableToPatch = _mapper.Map<TableUpdateDto>(tableEntity);
    
    patchDocument.ApplyTo(tableToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(tableToPatch))
    {
      return BadRequest(ModelState);
    }
    
    if (!await _restaurantRepository.IsExistAsync(tableToPatch.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(tableToPatch, tableEntity);

    await _tableRepository.UpdateAsync(tableEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing table specified by ID.
  /// </summary>
  /// <param name="id">The Id of the table to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the table with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteTable(int id)
  {
    try
    {
      await _tableRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}