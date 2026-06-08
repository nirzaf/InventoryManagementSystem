namespace InventoryManagementSystem.Core.Interfaces;

public interface IAgenticProcurementClient
{
    Task<AgenticProcurementDraft> DraftSupplierCorrespondenceAsync(
        int supplierId,
        IEnumerable<int> itemIds,
        CancellationToken cancellationToken = default);
}

public class AgenticProcurementDraft
{
    public int SupplierId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
