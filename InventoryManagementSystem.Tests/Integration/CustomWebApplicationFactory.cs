using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InventoryManagementSystem.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<InventoryManagementSystem.Web.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Run in Testing environment to skip SeedData and use Testing-specific settings
        builder.UseEnvironment("Testing");
    }
}
