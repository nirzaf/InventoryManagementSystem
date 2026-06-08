using InventoryManagementSystem.Core.Entities;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrder>> GetAllAsync();
    Task<IEnumerable<PurchaseOrder>> GetPagedAsync(int page, int pageSize);
    Task<int> GetCountAsync();
    Task<PurchaseOrder?> GetByIdAsync(int id);
    Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder, List<OrderDetail> details);
    Task UpdateStatusAsync(int id, string status);
    Task DeleteAsync(int id);
}
