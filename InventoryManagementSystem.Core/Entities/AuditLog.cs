using System;

namespace InventoryManagementSystem.Core.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public string? KeyValues { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? ChangedColumns { get; set; }
}
