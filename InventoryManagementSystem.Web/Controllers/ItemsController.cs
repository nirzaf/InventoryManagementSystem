using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers;

[Authorize]
public class ItemsController : Controller
{
    private readonly IItemService _itemService;
    private readonly ISupplierService _supplierService;

    public ItemsController(IItemService itemService, ISupplierService supplierService)
    {
        _itemService = itemService;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 20)
    {
        var items = string.IsNullOrWhiteSpace(search)
            ? await _itemService.GetPagedAsync(page, pageSize)
            : await _itemService.SearchAsync(search);

        ViewBag.Search = search;
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalCount = await _itemService.GetCountAsync();
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Suppliers = await _supplierService.GetAllAsync();
        return View(new Item());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Item item)
    {
        if (ModelState.IsValid)
        {
            await _itemService.CreateAsync(item);
            TempData["Success"] = "Item created successfully.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Suppliers = await _supplierService.GetAllAsync();
        return View(item);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();
        ViewBag.Suppliers = await _supplierService.GetAllAsync();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Item item)
    {
        if (id != item.Id) return NotFound();
        if (ModelState.IsValid)
        {
            await _itemService.UpdateAsync(item);
            TempData["Success"] = "Item updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Suppliers = await _supplierService.GetAllAsync();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _itemService.DeleteAsync(id);
        TempData["Success"] = "Item deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
