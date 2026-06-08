namespace InventoryManagementSystem.Core.Entities;

/// <summary>
/// Lifecycle statuses for purchase orders.
/// </summary>
public enum PurchaseOrderStatus
{
    Draft,
    Pending,
    Submitted,
    Approved,
    Received,
    Cancelled
}
