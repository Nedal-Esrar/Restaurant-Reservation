using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Models.Employees;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/employees")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class EmployeesController : ControllerBase
{
  private readonly IEmployeeRepository _employeeRepository;

  private readonly IRestaurantRepository _restaurantRepository;

  private readonly IOrderRepository _orderRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;

  public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository)
  {
    _employeeRepository = employeeRepository;

    _mapper = mapper;
    
    _restaurantRepository = restaurantRepository;
    
    _orderRepository = orderRepository;
  }

  /// <summary>
  /// Gets employees partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of employees with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeResponseDto>))]
  public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (employees, paginationMetadata) = await _employeeRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<EmployeeResponseDto>>(employees));
  }

  /// <summary>
  /// Returns an employee specified by ID.
  /// </summary>
  /// <param name="id">The ID of the employee to retrieve.</param>
  /// <response code="404">If the employee with the given id is not found.</response>
  /// <response code="200">Returns the requested employee.</response>
  [HttpGet("{id}", Name = "GetEmployee")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeResponseDto))]
  public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(int id)
  {
    var employee = await _employeeRepository.GetByIdAsync(id);

    if (employee is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<EmployeeResponseDto>(employee));
  }
  
  /// <summary>
  /// Gets employees who are managers partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns all employees that are managers.</response>
  [HttpGet("managers")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeResponseDto>))]
  public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetManagers(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }
    
    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (employees, paginationMetadata) = await _employeeRepository.GetAllAsync(e => e.Position == EmployeePosition.Manager, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<EmployeeResponseDto>>(employees));
  }

  /// <summary>
  /// Returns the average order amount for an employee.
  /// </summary>
  /// <param name="employeeId">The ID of the employee.</param>
  /// <response code="404">If the employee with the given ID is not found.</response>
  /// <response code="200">Returns the average order amount of the employee.</response>
  [HttpGet("{employeeId}/average-order-amount")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAverageOrderAmount(int employeeId)
  {
    try
    {
      var averageOrderAmount = await _orderRepository.CalculateAverageOrderAmountAsync(employeeId);

      return Ok(new { averageOrderAmount });
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
  }

  /// <summary>
  /// Creates a new employee.
  /// </summary>
  /// <param name="employeeCreationDto">The data of the new employee.</param>
  /// <returns>The newly created employee.</returns>
  /// <response code="400">If the employee data is invalid.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="201">If the employee is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmployeeResponseDto))]
  public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee(EmployeeCreationDto employeeCreationDto)
  {
    if (!await _restaurantRepository.IsExistAsync(employeeCreationDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }
    
    var employeeToAdd = _mapper.Map<Employee>(employeeCreationDto);

    var addedEmployee = await _employeeRepository.CreateAsync(employeeToAdd);

    return CreatedAtRoute(
      "GetEmployee",
      new { id = addedEmployee.Id },
      _mapper.Map<EmployeeResponseDto>(addedEmployee)
    );
  }

  /// <summary>
  /// Updates an existing employee specified by ID.
  /// </summary>
  /// <param name="id">The ID of the employee to update.</param>
  /// <param name="employeeUpdateDto">The data for updating the employee.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the employee with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateDto employeeUpdateDto)
  {
    var employeeEntity = await _employeeRepository.GetByIdAsync(id);

    if (employeeEntity is null)
    {
      return NotFound();
    }
    
    if (!await _restaurantRepository.IsExistAsync(employeeUpdateDto.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(employeeUpdateDto, employeeEntity);

    await _employeeRepository.UpdateAsync(employeeEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing employee specified by ID.
  /// </summary>
  /// <param name="id">The ID of the employee to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the employee with the specified ID is not found.</response>
  /// <response code="422">If there is an invalid foreign key in the request.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> PartiallyUpdateEmployee(int id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
  {
    var employeeEntity = await _employeeRepository.GetByIdAsync(id);

    if (employeeEntity is null)
    {
      return NotFound();
    }

    var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
    
    patchDocument.ApplyTo(employeeToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(employeeToPatch))
    {
      return BadRequest(ModelState);
    }
    
    if (!await _restaurantRepository.IsExistAsync(employeeToPatch.RestaurantId))
    {
      return UnprocessableEntity("Restaurant with the given ID is not found.");
    }

    _mapper.Map(employeeToPatch, employeeEntity);

    await _employeeRepository.UpdateAsync(employeeEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing employee specified by ID.
  /// </summary>
  /// <param name="id">The ID of the employee to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the employee with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  /// <response code="403">The user is not an admin.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> DeleteEmployee(int id)
  {
    try
    {
      await _employeeRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}