using System.Threading.Tasks;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IWebhookDispatcher
{
    Task DispatchAsync<T>(string eventType, T payload);
}
