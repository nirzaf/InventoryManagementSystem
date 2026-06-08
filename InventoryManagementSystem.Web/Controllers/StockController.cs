using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers;

[Authorize]
public class StockController : Controller
{
    private readonly IStockService _stockService;
    private readonly IItemService _itemService;
    private readonly ILocationService _locationService;

    public StockController(IStockService stockService, IItemService itemService, ILocationService locationService)
    {
        _stockService = stockService;
        _itemService = itemService;
        _locationService = locationService;
    }

    public async Task<IActionResult> Index()
    {
        var stocks = await _stockService.GetAllAsync();
        return View(stocks);
    }

    public async Task<IActionResult> Receive()
    {
        ViewBag.Items = await _itemService.GetAllAsync();
        ViewBag.Locations = await _locationService.GetAllAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Receive(int itemId, int locationId, int quantity, string? notes)
    {
        try
        {
            await _stockService.ReceiveStockAsync(itemId, locationId, quantity, notes);
            TempData["Success"] = $"Received {quantity} units successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Transfer()
    {
        ViewBag.Items = await _itemService.GetAllAsync();
        ViewBag.Locations = await _locationService.GetAllAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Transfer(int itemId, int fromLocationId, int toLocationId, int quantity, string? notes)
    {
        try
        {
            await _stockService.TransferStockAsync(itemId, fromLocationId, toLocationId, quantity, notes);
            TempData["Success"] = $"Transferred {quantity} units successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Sell()
    {
        ViewBag.Items = await _itemService.GetAllAsync();
        ViewBag.Locations = await _locationService.GetAllAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sell(int itemId, int locationId, int quantity, string? notes)
    {
        try
        {
            await _stockService.SellStockAsync(itemId, locationId, quantity, notes);
            TempData["Success"] = $"Sold {quantity} units successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Transactions(DateTime? from, DateTime? to)
    {
        var transactions = await _stockService.GetTransactionsAsync(from, to);
        return View(transactions);
    }
}
