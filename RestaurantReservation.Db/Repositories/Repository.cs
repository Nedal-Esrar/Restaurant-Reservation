using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Exceptions;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Utilities;

namespace RestaurantReservation.Db.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
  protected readonly RestaurantReservationDbContext Context;

  protected readonly DbSet<TEntity> DbSet;

  protected Repository(RestaurantReservationDbContext context)
  {
    Context = context ?? throw new ArgumentNullException(nameof(context));

    DbSet = Context.Set<TEntity>();
  }

  public async Task<(IEnumerable<TEntity>, PaginationMetadata)> GetAllAsync(Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize)
  {
    var itemsQueryable = DbSet.Where(filter);

    var paginationMetadata = new PaginationMetadata
    {
      TotalItemCount = await itemsQueryable.CountAsync(),
      PageSize = pageSize,
      CurrentPage = pageNumber
    };

    var items = await itemsQueryable
      .Skip(pageSize * (pageNumber - 1))
      .Take(pageSize)
      .ToListAsync();

    return (items, paginationMetadata);
  }

  public virtual async Task<TEntity> CreateAsync(TEntity entity)
  {
    if (entity is null) throw new ArgumentNullException();

    await DbSet.AddAsync(entity);

    await Context.SaveChangesAsync();

    return entity;
  }
  
  public virtual async Task UpdateAsync(TEntity entity)
  {
    if (entity is null) throw new ArgumentNullException();

    if (!await IsExistAsync(entity.Id))
      throw new NotFoundException(StandardMessages.GenerateNotFoundMessage(nameof(entity), entity.Id));

    DbSet.Update(entity);

    await Context.SaveChangesAsync();
  }

  public virtual async Task DeleteAsync(int id)
  {
    var entity = await Context.Set<TEntity>().FindAsync(id) ??
                 throw new NotFoundException(StandardMessages.GenerateNotFoundMessage("entity", id));

    DbSet.Remove(entity);

    await Context.SaveChangesAsync();
  }

  public virtual async Task<bool> IsExistAsync(int id)
  {
    return await DbSet.AnyAsync(e => e.Id == id);
  }
}