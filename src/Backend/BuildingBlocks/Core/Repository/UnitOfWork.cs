using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Repository;

public class UnitOfWork<TContext> : IDisposable, IUnitOfWork where TContext : DbContext
{
    public UnitOfWork(TContext context) => Context = context;

    public TContext Context { get; }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.RollbackTransactionAsync();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.CommitTransactionAsync(cancellationToken);
    }

    public void Dispose() => Context.Dispose();
}