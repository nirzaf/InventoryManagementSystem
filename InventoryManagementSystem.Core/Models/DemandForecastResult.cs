namespace InventoryManagementSystem.Core.Models;

public class DemandForecastResult
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public List<float> ForecastedValues { get; set; } = [];
    public List<float> ConfidenceLower { get; set; } = [];
    public List<float> ConfidenceUpper { get; set; } = [];
    public float AverageDailyDemand { get; set; }
    public int TotalHistoricalDays { get; set; }
    public int ForecastHorizonDays { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
