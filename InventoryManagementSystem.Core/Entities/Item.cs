using InventoryManagementSystem.Core.Interfaces;

namespace InventoryManagementSystem.Core.Entities;

public class Item : AuditableEntity, ISoftDelete
{
    public int Id { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<StockInHand> StockInHands { get; set; } = new List<StockInHand>();
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
