using InventoryManagementSystem.Core.Interfaces;

namespace InventoryManagementSystem.Core.Entities;

public class Location : AuditableEntity, ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<StockInHand> StockInHands { get; set; } = new List<StockInHand>();
}
