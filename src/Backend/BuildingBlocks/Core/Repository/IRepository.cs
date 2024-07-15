using System.Linq.Expressions;
using BuildingBlocks.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Repository;

public interface IRepository<T,TId> where T : Entity<TId>
{
    public DbSet<T> Table { get; }
    public Task<T?> GetByIdAsync(TId id);
    public void Update(T entity);
    public Task<T> CreateAsync(T entity);
    public void Delete(T entity);
    
    public IQueryable<T> Where(Expression<Func<T, bool>> entity);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> value);
}