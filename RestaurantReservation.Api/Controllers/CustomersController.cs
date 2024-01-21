using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Models.Customers;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/customers")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class CustomersController : ControllerBase
{
  private readonly ICustomerRepository _customerRepository;

  private readonly IMapper _mapper;

  private const int MaxPageSize = 20;

  public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
  {
    _customerRepository = customerRepository;

    _mapper = mapper;
  }

  /// <summary>
  /// Gets customers partitioned into pages.
  /// </summary>
  /// <param name="pageNumber">The number of the needed page.</param>
  /// <param name="pageSize">The size of the needed page.</param>
  /// <response code="400">When pageNumber or pageSize is less than zero.</response>
  /// <response code="200">Returns the requested page of customers with pagination metadata in the headers.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerResponseDto>))]
  public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomers(int pageNumber = 1, int pageSize = 10)
  {
    if (pageNumber < 1 || pageSize < 1)
    {
      return BadRequest($"'{nameof(pageNumber)}' and '{nameof(pageSize)}' must be greater than 0.");
    }

    pageSize = Math.Min(pageSize, MaxPageSize);
    
    var (customers, paginationMetadata) = await _customerRepository.GetAllAsync(_ => true, pageNumber, pageSize);

    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    
    return Ok(_mapper.Map<IEnumerable<CustomerResponseDto>>(customers));
  }

  /// <summary>
  /// Returns a customer specified by ID.
  /// </summary>
  /// <param name="id">The ID of the customer to retrieve.</param>
  /// <response code="404">If the customer with the given id is not found.</response>
  /// <response code="200">Returns the requested customer.</response>
  [HttpGet("{id}", Name = "GetCustomer")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseDto))]
  public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id)
  {
    var customer = await _customerRepository.GetByIdAsync(id);

    if (customer is null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<CustomerResponseDto>(customer));
  }

  /// <summary>
  /// Creates a new customer.
  /// </summary>
  /// <param name="customerCreationDto">The data of the new customer.</param>
  /// <returns>The newly created customer.</returns>
  /// <response code="400">If the creation data is invalid.</response>
  /// <response code="201">If the customer is created successfully.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerResponseDto))]
  public async Task<ActionResult<CustomerResponseDto>> CreateCustomer(CustomerCreationDto customerCreationDto)
  {
    var customerToAdd = _mapper.Map<Customer>(customerCreationDto);

    var addedCustomer = await _customerRepository.CreateAsync(customerToAdd);

    return CreatedAtRoute(
      "GetCustomer",
      new { id = addedCustomer.Id },
      _mapper.Map<CustomerResponseDto>(addedCustomer)
    );
  }

  /// <summary>
  /// Updates an existing customer specified by ID.
  /// </summary>
  /// <param name="id">The ID of the customer to update.</param>
  /// <param name="customerUpdateDto">The data for updating the customer.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the updating data is invalid.</response>
  /// <response code="404">If the customer with the specified ID is not found.</response>
  /// <response code="204">If successful.</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto customerUpdateDto)
  {
    var customerEntity = await _customerRepository.GetByIdAsync(id);

    if (customerEntity is null)
    {
      return NotFound();
    }

    _mapper.Map(customerUpdateDto, customerEntity);

    await _customerRepository.UpdateAsync(customerEntity);

    return NoContent();
  }

  /// <summary>
  /// Partially updates an existing customer specified by ID.
  /// </summary>
  /// <param name="id">The ID of the customer to update.</param>
  /// <param name="patchDocument">The JSON patch document with partial update operations.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="400">If the patch document or updated data is invalid.</response>
  /// <response code="404">If the customer with the specified ID is not found.</response>
  /// <response code="204">If the update is successful.</response>
  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PartiallyUpdateCustomer(int id, JsonPatchDocument<CustomerUpdateDto> patchDocument)
  {
    var customerEntity = await _customerRepository.GetByIdAsync(id);

    if (customerEntity is null)
    {
      return NotFound();
    }

    var customerToPatch = _mapper.Map<CustomerUpdateDto>(customerEntity);
    
    patchDocument.ApplyTo(customerToPatch, ModelState);

    if (!ModelState.IsValid || !TryValidateModel(customerToPatch))
    {
      return BadRequest(ModelState);
    }

    _mapper.Map(customerToPatch, customerEntity);

    await _customerRepository.UpdateAsync(customerEntity);

    return NoContent();
  }
  
  /// <summary>
  /// Deletes an existing customer specified by ID.
  /// </summary>
  /// <param name="id">The ID of the customer to delete.</param>
  /// <returns>No content if successful.</returns>
  /// <response code="404">if the customer with the specified ID is not found.</response>
  /// <response code="204">if the deletion is successful.</response>
  /// <response code="403">The user is not an admin.</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [Authorize(Roles = UserRoles.Admin)]
  public async Task<IActionResult> DeleteCustomer(int id)
  {
    try
    {
      await _customerRepository.DeleteAsync(id);
    }
    catch (NotFoundException)
    {
      return NotFound();
    }
    
    return NoContent();
  }
}