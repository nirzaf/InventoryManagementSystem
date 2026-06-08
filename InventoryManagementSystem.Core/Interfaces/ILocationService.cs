using InventoryManagementSystem.Core.Entities;

namespace InventoryManagementSystem.Core.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
    Task<Location> CreateAsync(Location location);
    Task UpdateAsync(Location location);
    Task DeleteAsync(int id);
}
