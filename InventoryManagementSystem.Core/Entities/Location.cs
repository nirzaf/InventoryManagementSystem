namespace InventoryManagementSystem.Core.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public ICollection<StockInHand> StockInHands { get; set; } = new List<StockInHand>();
}
