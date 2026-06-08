using InventoryManagementSystem.Core.Entities;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllAsync();
    Task<IEnumerable<Item>> GetPagedAsync(int page, int pageSize);
    Task<int> GetCountAsync();
    Task<Item?> GetByIdAsync(int id);
    Task<Item> CreateAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(int id);
    Task<IEnumerable<Item>> SearchAsync(string searchTerm);
}
