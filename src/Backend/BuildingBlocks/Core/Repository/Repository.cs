using System.Linq.Expressions;
using BuildingBlocks.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Repository;

public  class Repository<T,TId,TContext> : IRepository<T,TId> where T : Entity<TId> where TContext : DbContext
{
    public readonly TContext Db;

    public DbSet<T> Table { get => Db.Set<T>(); }

    public Repository(TContext db)
    {
        Db = db;
    }
    
    public virtual async Task<T?> GetByIdAsync(TId id)
    {
        return await Db.Set<T>().FindAsync(id);
    }

    public virtual void  Update(T entity)
    {
        Db.Set<T>().Update(entity);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        var e = await Db.Set<T>().AddAsync(entity);
        return e.Entity;
    }
    
    public virtual void Delete(T entity)
    {
        Db.Set<T>().Remove(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> entity)
    {
        return Db.Set<T>().Where(entity);
    }
}