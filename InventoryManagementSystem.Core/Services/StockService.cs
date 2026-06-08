using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Services;

public class StockService : IStockService
{
    private readonly IRepository<StockInHand> _stockRepo;
    private readonly IRepository<StockTransaction> _txRepo;
    private readonly IRepository<Item> _itemRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<StockService> _logger;

    public StockService(
        IRepository<StockInHand> stockRepo,
        IRepository<StockTransaction> txRepo,
        IRepository<Item> itemRepo,
        IUnitOfWork unitOfWork,
        ILogger<StockService> logger)
    {
        _stockRepo = stockRepo;
        _txRepo = txRepo;
        _itemRepo = itemRepo;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<StockInHand>> GetAllAsync() => await _stockRepo.GetAllAsync();

    public async Task<StockInHand?> GetByItemAndLocationAsync(int itemId, int locationId)
    {
        var results = await _stockRepo.FindAsync(s => s.ItemId == itemId && s.LocationId == locationId);
        return results.FirstOrDefault();
    }

    public async Task<IEnumerable<StockTransaction>> GetTransactionsAsync(DateTime? from, DateTime? to)
    {
        // Single composite predicate — fully executed on the database, zero in-memory filtering
        return await _txRepo.FindAsync(t =>
            (!from.HasValue || t.TransactionDate >= from.Value) &&
            (!to.HasValue || t.TransactionDate <= to.Value),
            orderBy: q => q.OrderByDescending(t => t.TransactionDate));
    }

    public async Task ReceiveStockAsync(int itemId, int locationId, int quantity, string? notes)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive");

        var existing = await GetByItemAndLocationAsync(itemId, locationId);
        if (existing != null)
        {
            existing.Quantity += quantity;
            await _stockRepo.UpdateAsync(existing);
        }
        else
        {
            await _stockRepo.AddAsync(new StockInHand
            {
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity
            });
        }

        await _txRepo.AddAsync(new StockTransaction
        {
            ItemId = itemId,
            FromLocationId = locationId,
            ToLocationId = locationId,
            Quantity = quantity,
            TransactionType = TransactionType.Receive,
            TransactionDate = DateTime.UtcNow,
            Notes = notes
        });

        // Single commit for atomicity
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Received {Qty} of item {ItemId} at location {LocId}", quantity, itemId, locationId);
    }

    public async Task TransferStockAsync(int itemId, int fromLocationId, int toLocationId, int quantity, string? notes)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive");
        if (fromLocationId == toLocationId) throw new ArgumentException("Source and destination must be different");

        var source = await GetByItemAndLocationAsync(itemId, fromLocationId);
        if (source == null || source.Quantity < quantity)
            throw new InvalidOperationException("Insufficient stock at source location");

        source.Quantity -= quantity;
        await _stockRepo.UpdateAsync(source);

        var dest = await GetByItemAndLocationAsync(itemId, toLocationId);
        if (dest != null)
        {
            dest.Quantity += quantity;
            await _stockRepo.UpdateAsync(dest);
        }
        else
        {
            await _stockRepo.AddAsync(new StockInHand
            {
                ItemId = itemId,
                LocationId = toLocationId,
                Quantity = quantity
            });
        }

        await _txRepo.AddAsync(new StockTransaction
        {
            ItemId = itemId,
            FromLocationId = fromLocationId,
            ToLocationId = toLocationId,
            Quantity = quantity,
            TransactionType = TransactionType.Transfer,
            TransactionDate = DateTime.UtcNow,
            Notes = notes
        });

        // Single commit for atomicity
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Transferred {Qty} of item {ItemId} from {From} to {To}", quantity, itemId, fromLocationId, toLocationId);
    }

    public async Task SellStockAsync(int itemId, int locationId, int quantity, string? notes)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive");

        var stock = await GetByItemAndLocationAsync(itemId, locationId);
        if (stock == null || stock.Quantity < quantity)
            throw new InvalidOperationException("Insufficient stock for sale");

        stock.Quantity -= quantity;
        await _stockRepo.UpdateAsync(stock);

        await _txRepo.AddAsync(new StockTransaction
        {
            ItemId = itemId,
            FromLocationId = locationId,
            Quantity = quantity,
            TransactionType = TransactionType.Sell,
            TransactionDate = DateTime.UtcNow,
            Notes = notes
        });

        // Single commit for atomicity
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Sold {Qty} of item {ItemId} from location {LocId}", quantity, itemId, locationId);
    }
}
