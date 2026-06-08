using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Core.Services;
using InventoryManagementSystem.Infrastructure.Data;
using InventoryManagementSystem.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;

namespace InventoryManagementSystem.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/inventory-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();

        // Database
        builder.Services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<InventoryDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        // MudBlazor
        builder.Services.AddMudServices();

        // Repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Services
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IStockService, StockService>();
        builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

        // MediatR CQRS — scans all handler assemblies
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(InventoryManagementSystem.Core.Features.Items.Queries.GetAllItemsQuery).Assembly));

        // MVC
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSerilogRequestLogging();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        // === Headless API v1 (MediatR-powered minimal endpoints) ===
        var api = app.MapGroup("/api/v1");

        api.MapGet("/items", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new InventoryManagementSystem.Core.Features.Items.Queries.GetAllItemsQuery())))
            .WithName("GetAllItems")
            .WithTags("Items");

        api.MapGet("/items/{id:int}", async (int id, IMediator mediator) =>
        {
            var item = await mediator.Send(new InventoryManagementSystem.Core.Features.Items.Queries.GetItemByIdQuery(id));
            return item is null ? Results.NotFound() : Results.Ok(item);
        })
            .WithName("GetItemById")
            .WithTags("Items");

        api.MapGet("/stock", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new InventoryManagementSystem.Core.Features.Stock.Queries.GetAllStockQuery())))
            .WithName("GetAllStock")
            .WithTags("Stock");

        api.MapPost("/stock/receive", async (InventoryManagementSystem.Core.Features.Stock.Commands.ReceiveStockCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.NoContent();
        })
            .WithName("ReceiveStock")
            .WithTags("Stock");

        // Seed demo data in development only
        if (app.Environment.IsDevelopment())
        {
            await SeedData.Initialize(app.Services);
        }

        await app.RunAsync();
    }
}
