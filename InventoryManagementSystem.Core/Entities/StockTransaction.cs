namespace InventoryManagementSystem.Core.Entities;

public class StockTransaction
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int FromLocationId { get; set; }
    public Location FromLocation { get; set; } = null!;
    public int? ToLocationId { get; set; }
    public Location? ToLocation { get; set; }
    public int Quantity { get; set; }
    public string TransactionType { get; set; } = string.Empty; // Receive, Transfer, Sell
    public DateTime TransactionDate { get; set; }
    public string? Notes { get; set; }
}
