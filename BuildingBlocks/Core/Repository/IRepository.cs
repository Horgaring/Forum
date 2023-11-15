using BuildingBlocks.Core.Model;

namespace BuildingBlocks.Core.Repository;

public interface IRepository<T,TId> where T : Entity<TId>
{
    public Task<T?> GetByIdAsync(TId id);
    public void Update(T entity);
    public Task<T> CreateAsync(T entity);
    public void Delete(T entity);
}