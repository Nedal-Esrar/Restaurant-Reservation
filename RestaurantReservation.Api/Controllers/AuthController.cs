using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Interfaces;
using RestaurantReservation.Api.Models.Auth;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Api.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IUserRepository _userRepository;

  private readonly IRoleRepository _roleRepository;

  private readonly IJwtTokenGenerator _jwtTokenGenerator;

  private readonly IMapper _mapper;

  public AuthController(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, IRoleRepository roleRepository)
  {
    _userRepository = userRepository;
    
    _jwtTokenGenerator = jwtTokenGenerator;
    
    _mapper = mapper;
    
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Processes a login request.
  /// </summary>
  /// <param name="loginRequestDto">Login request data.</param>
  /// <response code="200">JWT token.</response>
  /// <response code="400">If the login credentials are invalid.</response>
  /// <response code="401">When user with the provided credentials does not exist.</response>
  [HttpPost("login")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtToken))]
  public async Task<ActionResult<JwtToken>> Login(LoginRequestDto loginRequestDto)
  {
    var authenticatedUser = await _userRepository.AuthenticateAsync(loginRequestDto.Username, loginRequestDto.Password);

    if (authenticatedUser is null)
    {
      return Unauthorized();
    }

    var userWithoutPassword = _mapper.Map<UserWithoutPasswordDto>(authenticatedUser);

    return Ok(_jwtTokenGenerator.GenerateToken(userWithoutPassword));
  }

  /// <summary>
  /// Processes registering a normal user request.
  /// </summary>
  /// <param name="registerRequestDto">Registering request data.</param>
  /// <returns>JWT token</returns>
  /// <response code="200">JWT token.</response>
  /// <response code="400">If the register data are invalid or the user name is duplicated.</response>
  [HttpPost("register-user")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtToken))]
  public async Task<ActionResult<JwtToken>> RegisterUser(RegisterRequestDto registerRequestDto)
  {
    if (await _userRepository.IsExistByUsernameAsync(registerRequestDto.Username))
    {
      return BadRequest("Username already exists.");
    }

    var user = _mapper.Map<User>(registerRequestDto);
    
    user.Roles.Add(await _roleRepository.GetByName(UserRoles.User));

    var addedUser = await _userRepository.CreateAsync(user);

    var userWithoutPassword = _mapper.Map<UserWithoutPasswordDto>(addedUser);

    return Ok(_jwtTokenGenerator.GenerateToken(userWithoutPassword));
  }
  
  /// <summary>
  /// Processes registering an admin user request.
  /// </summary>
  /// <param name="registerRequestDto">Registering request data.</param>
  /// <returns>JWT token</returns>
  /// <response code="200">JWT token.</response>
  /// <response code="400">If the register data are invalid or the user name is duplicated.</response>
  [HttpPost("register-admin")]
  public async Task<ActionResult<JwtToken>> RegisterAdmin(RegisterRequestDto registerRequestDto)
  {
    if (await _userRepository.IsExistByUsernameAsync(registerRequestDto.Username))
    {
      return BadRequest("Username already exists.");
    }

    var user = _mapper.Map<User>(registerRequestDto);
    
    user.Roles.Add(await _roleRepository.GetByName(UserRoles.Admin));

    var addedUser = await _userRepository.CreateAsync(user);

    var userWithoutPassword = _mapper.Map<UserWithoutPasswordDto>(addedUser);

    return Ok(_jwtTokenGenerator.GenerateToken(userWithoutPassword));
  }
}