using BuildingBlocks.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Repository;

public class Repository<T,TId,TContext> : IRepository<T,TId> where T : Entity<TId> where TContext : DbContext
{
    protected readonly TContext _db;

    public Repository(TContext db)
    {
        _db = db;
    }
    
    public async Task<T?> GetByIdAsync(TId id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    public void  Update(T entity)
    {
        _db.Set<T>().Update(entity);
    }

    public async Task<T> CreateAsync(T entity)
    {
        var e = await _db.Set<T>().AddAsync(entity);
        return e.Entity;
    }

    public  void Delete(T entity)
    {
        _db.Set<T>().Remove(entity);
    }
}