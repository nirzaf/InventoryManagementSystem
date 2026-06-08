using InventoryManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IItemService _itemService;
    private readonly IStockService _stockService;
    private readonly IPurchaseOrderService _poService;

    public HomeController(IItemService itemService, IStockService stockService, IPurchaseOrderService poService)
    {
        _itemService = itemService;
        _stockService = stockService;
        _poService = poService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ItemCount = await _itemService.GetCountAsync();
        ViewBag.POCount = await _poService.GetCountAsync();
        ViewBag.StockItems = (await _stockService.GetAllAsync()).Sum(s => s.Quantity);
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
