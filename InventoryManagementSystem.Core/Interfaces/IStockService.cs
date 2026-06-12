using InventoryManagementSystem.Core.Entities;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IStockService
{
    Task<IEnumerable<StockInHand>> GetAllAsync();
    Task<StockInHand?> GetByItemAndLocationAsync(int itemId, int locationId);
    Task<IEnumerable<StockTransaction>> GetTransactionsAsync(DateTime? from, DateTime? to);
    Task ReceiveStockAsync(int itemId, int locationId, int quantity, string? notes, string? batchNumber = null, DateTime? expiryDate = null);
    Task TransferStockAsync(int itemId, int fromLocationId, int toLocationId, int quantity, string? notes, string? batchNumber = null, DateTime? expiryDate = null);
    Task SellStockAsync(int itemId, int locationId, int quantity, string? notes, string? batchNumber = null, DateTime? expiryDate = null);
}
