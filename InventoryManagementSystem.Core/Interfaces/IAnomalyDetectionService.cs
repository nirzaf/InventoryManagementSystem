using InventoryManagementSystem.Core.Models;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IAnomalyDetectionService
{
    Task<IReadOnlyList<StockAnomaly>> DetectAnomaliesAsync(DateTime? from = null, DateTime? to = null);
}
