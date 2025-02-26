using Microsoft.AspNetCore.Mvc;
using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AutoFusion.Web.Controllers;

[Authorize(Roles = nameof(AccessLevel.Administrator))]
[Route("vehicle")]
public class VehicleController(IVehicleService vehicleService, IManufacturerService manufacturerService) : Controller
{
    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IManufacturerService _manufacturerService = manufacturerService;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        return View(vehicles);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = manufacturers ?? new List<Manufacturer>();
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Vehicle vehicle)
    {
        ModelState.Remove(nameof(vehicle.Manufacturer));
        if (!ModelState.IsValid)
        {
            ViewBag.Manufacturers = await _manufacturerService.GetAllAsync();
            return View(vehicle);
        }

        try
        {
            await _vehicleService.AddAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Manufacturers = await _manufacturerService.GetAllAsync();
            return View(vehicle);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null)
            return NotFound();

        ViewBag.Manufacturers = await _manufacturerService.GetAllAsync();
        return View(vehicle);
    }

    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Vehicle vehicle)
    {
        if (id != vehicle.VehicleId)
            return BadRequest("ID do veículo não corresponde.");

        ModelState.Remove(nameof(vehicle.Manufacturer));
        if (!ModelState.IsValid)
        {
            ViewBag.Manufacturers = await _manufacturerService.GetAllAsync();
            return View(vehicle);
        }

        try
        {
            await _vehicleService.UpdateAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Manufacturers = await _manufacturerService.GetAllAsync();
            return View(vehicle);
        }
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null)
            return NotFound();

        return View(vehicle);
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle == null)
            return NotFound();

        return View(vehicle);
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _vehicleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var vehicle = await _vehicleService.GetByIdAsync(id);
            return View(vehicle);
        }
    }
}
