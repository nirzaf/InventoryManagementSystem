using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers;

[Authorize]
public class LocationsController : Controller
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<IActionResult> Index()
    {
        var locations = await _locationService.GetAllAsync();
        return View(locations);
    }

    public IActionResult Create() => View(new Location());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Location location)
    {
        if (ModelState.IsValid)
        {
            await _locationService.CreateAsync(location);
            TempData["Success"] = "Location created successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(location);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var location = await _locationService.GetByIdAsync(id);
        if (location == null) return NotFound();
        return View(location);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Location location)
    {
        if (id != location.Id) return NotFound();
        if (ModelState.IsValid)
        {
            await _locationService.UpdateAsync(location);
            TempData["Success"] = "Location updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(location);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _locationService.DeleteAsync(id);
        TempData["Success"] = "Location deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
