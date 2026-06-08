using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers;

[Authorize]
public class PurchaseOrdersController : Controller
{
    private readonly IPurchaseOrderService _poService;
    private readonly ISupplierService _supplierService;
    private readonly IItemService _itemService;

    public PurchaseOrdersController(
        IPurchaseOrderService poService,
        ISupplierService supplierService,
        IItemService itemService)
    {
        _poService = poService;
        _supplierService = supplierService;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
    {
        var pos = await _poService.GetPagedAsync(page, pageSize);
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalCount = await _poService.GetCountAsync();
        return View(pos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var po = await _poService.GetByIdAsync(id);
        if (po == null) return NotFound();
        return View(po);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Suppliers = await _supplierService.GetAllAsync();
        ViewBag.Items = await _itemService.GetAllAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseOrder po, int[] itemIds, int[] quantities, decimal[] unitPrices)
    {
        var details = new List<OrderDetail>();
        for (int i = 0; i < itemIds.Length; i++)
        {
            if (quantities[i] > 0)
            {
                details.Add(new OrderDetail
                {
                    ItemId = itemIds[i],
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i]
                });
            }
        }

        if (details.Count == 0)
        {
            TempData["Error"] = "Please add at least one item.";
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
            ViewBag.Items = await _itemService.GetAllAsync();
            return View(po);
        }

        await _poService.CreateAsync(po, details);
        TempData["Success"] = "Purchase order created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _poService.UpdateStatusAsync(id, status);
        TempData["Success"] = $"PO status updated to {status}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _poService.DeleteAsync(id);
        TempData["Success"] = "Purchase order deleted.";
        return RedirectToAction(nameof(Index));
    }
}
