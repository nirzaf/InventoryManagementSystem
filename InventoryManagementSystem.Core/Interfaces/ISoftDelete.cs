namespace InventoryManagementSystem.Core.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}
