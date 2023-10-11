using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public abstract class Repository<TEntity> where TEntity : Entity
{
  protected readonly RestaurantReservationDbContext Context;

  protected Repository(RestaurantReservationDbContext context)
  {
    Context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public virtual async Task CreateAsync(TEntity entity)
  {
    if (entity is null) throw new ArgumentNullException();

    await Context.Set<TEntity>().AddAsync(entity);

    await Context.SaveChangesAsync();
  }
  
  public virtual async Task UpdateAsync(TEntity entity)
  {
    if (entity is null) throw new ArgumentNullException();

    if (!await IsExistAsync(entity.Id))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage(nameof(entity), entity.Id));

    Context.Set<TEntity>().Update(entity);

    await Context.SaveChangesAsync();
  }

  public virtual async Task DeleteAsync(int id)
  {
    var entity = await Context.Set<TEntity>().FindAsync(id) ??
                 throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("entity", id));

    Context.Set<TEntity>().Remove(entity);

    await Context.SaveChangesAsync();
  }

  public virtual async Task<bool> IsExistAsync(int id)
  {
    return await Context.Set<TEntity>().AnyAsync(e => e.Id == id);
  }
}