using System.Threading.RateLimiting;
using Asp.Versioning;
using FluentValidation;
using InventoryManagementSystem.Core.Behaviors;
using InventoryManagementSystem.Core.Features.Items.Queries;
using InventoryManagementSystem.Core.Features.Stock.Commands;
using InventoryManagementSystem.Core.Features.Stock.Queries;
using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Core.Services;
using InventoryManagementSystem.Core.Validators;
using InventoryManagementSystem.Infrastructure.Data;
using InventoryManagementSystem.Infrastructure.Repositories;
using InventoryManagementSystem.Web.Middleware;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
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

        // Database (skip PostgreSQL in Testing — replaced by InMemory in test factory)
        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddDbContext<InventoryDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsql => npgsql.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)));
        }

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

        // HTTP context accessor for audit fields
        builder.Services.AddHttpContextAccessor();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Default", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Rate limiting
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            // API endpoints: 100 requests/minute
            options.AddFixedWindowLimiter("Api", limiter =>
            {
                limiter.PermitLimit = 100;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.QueueLimit = 10;
            });
            // AI endpoints: 10 requests/minute (CPU-intensive ML.NET)
            options.AddFixedWindowLimiter("Ai", limiter =>
            {
                limiter.PermitLimit = 10;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.QueueLimit = 2;
            });
        });

        // Repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Unit of Work — single commit boundary for multi-repository operations
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IStockService, StockService>();
        builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

        // AI / ML.NET services (platform-independent, no Azure)
        builder.Services.AddScoped<IDemandForecastService, DemandForecastService>();
        builder.Services.AddScoped<IAnomalyDetectionService, AnomalyDetectionService>();

        // FluentValidation — auto-validates MediatR requests via pipeline behavior
        builder.Services.AddValidatorsFromAssemblyContaining<ItemValidator>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // MediatR CQRS — scans all handler assemblies
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(InventoryManagementSystem.Core.Features.Items.Queries.GetAllItemsQuery).Assembly));

        // MVC & Blazor
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // API Versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"));
        }).AddMvc();

        // Global exception handling
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<InventoryDbContext>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
        {
            app.UseExceptionHandler("/Home/Error");

            // HSTS only when HTTPS is enabled (skip in Docker/reverse-proxy setups)
            if (string.IsNullOrEmpty(builder.Configuration["DISABLE_HTTPS"]))
            {
                app.UseHsts();
            }
        }
        else
        {
            // Still add exception handler middleware (without redirect path) to invoke IExceptionHandlers
            app.UseExceptionHandler(new ExceptionHandlerOptions { AllowStatusCode404Response = true });
        }

        // Skip HTTPS redirection in Docker or reverse-proxy deployments
        if (string.IsNullOrEmpty(builder.Configuration["DISABLE_HTTPS"]))
        {
            app.UseHttpsRedirection();
        }
        app.UseStaticFiles();
        app.UseSecurityHeaders();
        app.UseSerilogRequestLogging();

        app.UseRouting();
        app.UseRateLimiter();
        app.UseCors("Default");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();
        app.MapHealthChecks("/health");

        // === Headless API v1 (MediatR-powered minimal endpoints for AI/ML only) ===
        var v1 = app.MapGroup("/api/v1")
            .WithTags("API v1")
            .RequireAuthorization()
            .RequireRateLimiting("Api");

        // === AI / ML endpoints (rate-limited separately) ===
        var ai = app.MapGroup("/api/v1")
            .WithTags("AI")
            .RequireAuthorization()
            .RequireRateLimiting("Ai");

        ai.MapGet("/forecast/{itemId:int}", async (int itemId, int? horizon, IMediator mediator) =>
            Results.Ok(await mediator.Send(new ForecastDemandQuery(itemId, horizon ?? 30))))
            .WithName("ForecastDemand");

        ai.MapGet("/forecast", async (int? horizon, IMediator mediator) =>
            Results.Ok(await mediator.Send(new ForecastAllItemsDemandQuery(horizon ?? 30))))
            .WithName("ForecastAllDemand");

        ai.MapGet("/anomalies", async (DateTime? from, DateTime? to, IMediator mediator) =>
            Results.Ok(await mediator.Send(new DetectAnomaliesQuery(from, to))))
            .WithName("DetectAnomalies");

        // Auto-apply EF Core migrations (skip in Testing)
        if (!app.Environment.IsEnvironment("Testing"))
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            await db.Database.MigrateAsync();
        }

        // Seed demo data in development only
        if (app.Environment.IsDevelopment())
        {
            await SeedData.Initialize(app.Services);
        }

        await app.RunAsync();
    }
}
