namespace InventoryManagementSystem.Core.Interfaces;

/// <summary>
/// Coordinates persistence across multiple repositories in a single transaction.
/// Services should inject IUnitOfWork for multi-operation atomicity.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
