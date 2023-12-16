using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories;

public class RoleRepository : IRoleRepository
{
  private readonly RestaurantReservationDbContext _context;

  private readonly DbSet<Role> _dbSet;

  public RoleRepository(RestaurantReservationDbContext context)
  {
    _context = context;

    _dbSet = _context.Roles;
  }

  public Task<Role?> GetByName(string name)
  {
    return _dbSet.SingleOrDefaultAsync(r => r.Name == name);
  }
}