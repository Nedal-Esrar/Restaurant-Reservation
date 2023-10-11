using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository : Repository<Table>
{
  public TableRepository(RestaurantReservationDbContext context) : base(context)
  {
  }
  
  public override async Task DeleteAsync(int id)
  {
    if (!await IsExistAsync(id))
    {
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("Table", id));
    }

    var table = await Context.Tables
      .Include(t => t.Reservations)
      .FirstAsync(t => t.Id == id);
    
    foreach (var tableReservation in table.Reservations)
    {
      tableReservation.TableId = null;
    }
    
    Context.Tables.Remove(table);
  
    await Context.SaveChangesAsync();
  }
}