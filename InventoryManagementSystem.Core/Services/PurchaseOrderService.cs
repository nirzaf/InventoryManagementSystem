using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IRepository<PurchaseOrder> _poRepo;
    private readonly ILogger<PurchaseOrderService> _logger;

    public PurchaseOrderService(IRepository<PurchaseOrder> poRepo, ILogger<PurchaseOrderService> logger)
    {
        _poRepo = poRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllAsync() => await _poRepo.GetAllAsync();
    public async Task<IEnumerable<PurchaseOrder>> GetPagedAsync(int page, int pageSize) => await _poRepo.GetPagedAsync(page, pageSize);
    public async Task<int> GetCountAsync() => await _poRepo.CountAsync();

    public async Task<PurchaseOrder?> GetByIdAsync(int id)
    {
        // Note: This relies on lazy loading or Include in a real implementation
        return await _poRepo.GetByIdAsync(id);
    }

    public async Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder, List<OrderDetail> details)
    {
        purchaseOrder.OrderDate = DateTime.UtcNow;
        purchaseOrder.Status = "Pending";
        purchaseOrder.TotalAmount = details.Sum(d => d.Quantity * d.UnitPrice);
        purchaseOrder.OrderDetails = details;

        _logger.LogInformation("Creating PO {PONumber}", purchaseOrder.PONumber);
        return await _poRepo.AddAsync(purchaseOrder);
    }

    public async Task UpdateStatusAsync(int id, string status)
    {
        var po = await _poRepo.GetByIdAsync(id);
        if (po == null) throw new InvalidOperationException("Purchase order not found");

        po.Status = status;
        await _poRepo.UpdateAsync(po);
        _logger.LogInformation("Updated PO {Id} status to {Status}", id, status);
    }

    public async Task DeleteAsync(int id)
    {
        var po = await _poRepo.GetByIdAsync(id);
        if (po != null)
        {
            _logger.LogInformation("Deleting PO {Id}", id);
            await _poRepo.DeleteAsync(po);
        }
    }
}
