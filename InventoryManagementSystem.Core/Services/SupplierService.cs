using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Services;

public class SupplierService : ISupplierService
{
    private readonly IRepository<Supplier> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(IRepository<Supplier> repo, IUnitOfWork unitOfWork, ILogger<SupplierService> logger)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Supplier?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        _logger.LogInformation("Creating supplier {Name}", supplier.Name);
        var created = await _repo.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
        return created;
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _logger.LogInformation("Updating supplier {Id}", supplier.Id);
        await _repo.UpdateAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await _repo.GetByIdAsync(id);
        if (supplier != null)
        {
            _logger.LogInformation("Deleting supplier {Id}", id);
            await _repo.DeleteAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
