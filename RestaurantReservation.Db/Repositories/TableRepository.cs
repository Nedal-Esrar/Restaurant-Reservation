using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository : ITableRepository
{
  private readonly RestaurantReservationDbContext _context;

  public TableRepository(RestaurantReservationDbContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task CreateAsync(Table table)
  {
    if (table is null) throw new ArgumentNullException(nameof(table));

    await _context.Tables.AddAsync(table);

    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Table table)
  {
    if (table is null) throw new ArgumentNullException(nameof(table));

    if (!await IsExistAsync(table.TableId))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Table", table.TableId));

    _context.Tables.Update(table);

    await _context.SaveChangesAsync();
  }

  public async Task<bool> IsExistAsync(int id)
  {
    return await _context.Tables.AnyAsync(r => r.TableId == id);
  }

  public async Task DeleteAsync(int id)
  {
    var table = await _context.Tables.FindAsync(id) ??
                throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Table", id));

    _context.Tables.Remove(table);

    await _context.SaveChangesAsync();
  }
}