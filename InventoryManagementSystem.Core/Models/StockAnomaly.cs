namespace InventoryManagementSystem.Core.Models;

public class StockAnomaly
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public float ActualValue { get; set; }
    public float ExpectedValue { get; set; }
    public double ConfidenceScore { get; set; }
    public string AnomalyType { get; set; } = string.Empty; // Spike, Drop
}
