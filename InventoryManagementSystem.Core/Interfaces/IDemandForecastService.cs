using InventoryManagementSystem.Core.Models;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IDemandForecastService
{
    Task<DemandForecastResult> ForecastDemandAsync(int itemId, int horizonDays = 30);
    Task<IReadOnlyList<DemandForecastResult>> ForecastAllItemsAsync(int horizonDays = 30);
}
