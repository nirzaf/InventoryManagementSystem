using InventoryManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InventoryManagementSystem.Infrastructure.Data;

public class InventoryDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public InventoryDbContext(
        DbContextOptions<InventoryDbContext> options,
        IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<StockInHand> StockInHand { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<StockTransaction> StockTransactions { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = currentUser;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasIndex(e => e.ItemCode).IsUnique();
            entity.Property(e => e.ItemCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");

            entity.HasOne(i => i.Supplier)
                  .WithMany(s => s.Items)
                  .HasForeignKey(i => i.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ContactPerson).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
        });

        modelBuilder.Entity<StockInHand>(entity =>
        {
            entity.HasIndex(e => new { e.ItemId, e.LocationId }).IsUnique();

            entity.HasOne(s => s.Item)
                  .WithMany(i => i.StockInHands)
                  .HasForeignKey(s => s.ItemId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Location)
                  .WithMany(l => l.StockInHands)
                  .HasForeignKey(s => s.LocationId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property<uint>("Version")
                  .HasColumnName("xmin")
                  .HasColumnType("xid")
                  .ValueGeneratedOnAddOrUpdate()
                  .IsConcurrencyToken();
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasIndex(e => e.PONumber).IsUnique();
            entity.Property(e => e.PONumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);

            entity.HasOne(po => po.Supplier)
                  .WithMany(s => s.PurchaseOrders)
                  .HasForeignKey(po => po.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(od => od.PurchaseOrder)
                  .WithMany(po => po.OrderDetails)
                  .HasForeignKey(od => od.PurchaseOrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(od => od.Item)
                  .WithMany(i => i.OrderDetails)
                  .HasForeignKey(od => od.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.Property(e => e.TransactionType).HasConversion<string>().HasMaxLength(50).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(st => st.Item)
                  .WithMany(i => i.StockTransactions)
                  .HasForeignKey(st => st.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(st => st.FromLocation)
                  .WithMany()
                  .HasForeignKey(st => st.FromLocationId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(st => st.ToLocation)
                  .WithMany()
                  .HasForeignKey(st => st.ToLocationId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
