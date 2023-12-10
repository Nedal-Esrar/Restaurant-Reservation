using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories;

public class UserRepository : IUserRepository
{
  private readonly RestaurantReservationDbContext _context;

  private readonly DbSet<User> _dbSet;

  private readonly IPasswordHasher _passwordHasher;
  
  public UserRepository(RestaurantReservationDbContext context, IPasswordHasher passwordHasher)
  {
    _context = context;
    
    _passwordHasher = passwordHasher;

    _dbSet = _context.Users;
  }

  public async Task<User?> AuthenticateAsync(string username, string password)
  {
    var user = await _dbSet.Include(u => u.Roles)
      .SingleOrDefaultAsync(u => u.Username == username);

    if (user is null)
      return null;

    var passwordVerificationResult = _passwordHasher.
      VerifyHashedPassword(user.Password, password);

    return passwordVerificationResult == PasswordVerificationResult.Failed ? null : user;
  }

  public async Task<bool> IsExistByUsernameAsync(string username)
  {
    return await _dbSet.AnyAsync(u => u.Username == username);
  }

  public async Task<User> CreateAsync(User user)
  {
    user.Password = _passwordHasher.HashPassword(user.Password);

    await _dbSet.AddAsync(user);

    await _context.SaveChangesAsync();

    return user;
  }
}