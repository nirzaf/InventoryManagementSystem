using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Services;

public class ItemService : IItemService
{
    private readonly IRepository<Item> _repo;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IRepository<Item> repo, ILogger<ItemService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Item>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<IEnumerable<Item>> GetPagedAsync(int page, int pageSize) => await _repo.GetPagedAsync(page, pageSize);
    public async Task<int> GetCountAsync() => await _repo.CountAsync();
    public async Task<Item?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Item> CreateAsync(Item item)
    {
        _logger.LogInformation("Creating item {ItemCode}", item.ItemCode);
        return await _repo.AddAsync(item);
    }

    public async Task UpdateAsync(Item item)
    {
        _logger.LogInformation("Updating item {Id}", item.Id);
        await _repo.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        if (item != null)
        {
            _logger.LogInformation("Deleting item {Id}", id);
            await _repo.DeleteAsync(item);
        }
    }

    public async Task<IEnumerable<Item>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _repo.FindAsync(i =>
            i.ItemCode.ToLower().Contains(term) ||
            i.Description.ToLower().Contains(term));
    }
}
