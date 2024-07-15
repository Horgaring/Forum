namespace BuildingBlocks.Core.Repository;

public interface IUnitOfWork
{
    

    public Task<int> CommitAsync(CancellationToken cancellationToken = default);

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}