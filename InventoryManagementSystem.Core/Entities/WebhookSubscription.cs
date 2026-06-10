namespace InventoryManagementSystem.Core.Entities;

public class WebhookSubscription
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string EventType { get; set; } = null!; // e.g. "Stock.Received", "Stock.Transferred", "Stock.Sold", "*"
    public bool IsActive { get; set; } = true;
    public string? Secret { get; set; } // Used to sign the payload (HMAC-SHA256)
}
