using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Services;

public class LocationService : ILocationService
{
    private readonly IRepository<Location> _repo;
    private readonly ILogger<LocationService> _logger;

    public LocationService(IRepository<Location> repo, ILogger<LocationService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Location>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Location?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Location> CreateAsync(Location location)
    {
        _logger.LogInformation("Creating location {Name}", location.Name);
        return await _repo.AddAsync(location);
    }

    public async Task UpdateAsync(Location location)
    {
        _logger.LogInformation("Updating location {Id}", location.Id);
        await _repo.UpdateAsync(location);
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _repo.GetByIdAsync(id);
        if (location != null)
        {
            _logger.LogInformation("Deleting location {Id}", id);
            await _repo.DeleteAsync(location);
        }
    }
}
