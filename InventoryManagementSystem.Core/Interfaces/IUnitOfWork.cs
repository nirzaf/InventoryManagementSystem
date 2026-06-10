namespace InventoryManagementSystem.Core.Interfaces;

/// <summary>
/// Coordinates persistence across multiple repositories in a single transaction.
/// Services should inject IUnitOfWork for multi-operation atomicity.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    void ClearTracker();
}
