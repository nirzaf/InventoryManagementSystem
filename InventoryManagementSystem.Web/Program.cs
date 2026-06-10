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
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

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

        // Authentication Configuration (Cookies + JWT Bearer)
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? "SuperSecretKeyForDevelopmentPurposesOnlyDoNotUseInProduction123!";
        var key = Encoding.ASCII.GetBytes(secretKey);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "InventoryManagementSystem",
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"] ?? "InventoryManagementSystem",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // MudBlazor
        builder.Services.AddMudServices();

        // HTTP context accessor for audit fields
        builder.Services.AddHttpContextAccessor();

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

        // CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Default", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

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

        // Rate limiting
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("ApiLimit", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1);
                opt.PermitLimit = 60;
                opt.QueueLimit = 10;
                opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            });
        });

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<InventoryDbContext>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");

            // HSTS only when HTTPS is enabled (skip in Docker/reverse-proxy setups)
            if (string.IsNullOrEmpty(builder.Configuration["DISABLE_HTTPS"]))
            {
                app.UseHsts();
            }
        }

        // Skip HTTPS redirection in Docker or reverse-proxy deployments
        if (string.IsNullOrEmpty(builder.Configuration["DISABLE_HTTPS"]))
        {
            app.UseHttpsRedirection();
        }
        app.UseStaticFiles();
        app.UseSerilogRequestLogging();

        app.UseRouting();
        app.UseCors("Default");
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();
        app.MapHealthChecks("/health");

        var v1 = app.MapGroup("/api/v1")
            .WithTags("API v1")
            .RequireAuthorization(new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build())
            .RequireRateLimiting("ApiLimit");

        v1.MapPost("/auth/token", async (TokenRequest req, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
        {
            var user = await userManager.FindByNameAsync(req.Username) ?? await userManager.FindByEmailAsync(req.Username);
            if (user == null) return Results.Unauthorized();

            var result = await signInManager.CheckPasswordSignInAsync(user, req.Password, false);
            if (!result.Succeeded) return Results.Unauthorized();

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName ?? ""),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Staff")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = jwtSettings["Issuer"] ?? "InventoryManagementSystem",
                Audience = jwtSettings["Audience"] ?? "InventoryManagementSystem",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Results.Ok(new { Token = tokenHandler.WriteToken(token), Expires = tokenDescriptor.Expires });
        })
        .AllowAnonymous()
        .WithName("GenerateToken")
        .WithTags("Auth");

        v1.MapGet("/items", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllItemsQuery())))
            .WithName("GetAllItems")
            .WithTags("Items");

        v1.MapGet("/items/{id:int}", async (int id, IMediator mediator) =>
        {
            var item = await mediator.Send(new GetItemByIdQuery(id));
            return item is null ? Results.NotFound() : Results.Ok(item);
        })
            .WithName("GetItemById")
            .WithTags("Items");

        v1.MapGet("/stock", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllStockQuery())))
            .WithName("GetAllStock")
            .WithTags("Stock");

        v1.MapPost("/stock/receive", async (ReceiveStockCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd);
            return Results.NoContent();
        })
            .WithName("ReceiveStock")
            .WithTags("Stock")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager", "Staff"));

        // === AI / ML endpoints ===

        v1.MapGet("/forecast/{itemId:int}", async (int itemId, int? horizon, IMediator mediator) =>
            Results.Ok(await mediator.Send(new ForecastDemandQuery(itemId, horizon ?? 30))))
            .WithName("ForecastDemand")
            .WithTags("AI");

        v1.MapGet("/forecast", async (int? horizon, IMediator mediator) =>
            Results.Ok(await mediator.Send(new ForecastAllItemsDemandQuery(horizon ?? 30))))
            .WithName("ForecastAllDemand")
            .WithTags("AI");

        v1.MapGet("/anomalies", async (DateTime? from, DateTime? to, IMediator mediator) =>
            Results.Ok(await mediator.Send(new DetectAnomaliesQuery(from, to))))
            .WithName("DetectAnomalies")
            .WithTags("AI");

        // Auto-apply EF Core migrations (Development only)
        if (app.Environment.IsDevelopment())
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

public record TokenRequest(string Username, string Password);
