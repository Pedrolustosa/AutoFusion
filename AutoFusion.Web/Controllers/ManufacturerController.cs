using Microsoft.AspNetCore.Mvc;
using AutoFusion.Domain.Entities;
using AutoFusion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AutoFusion.Web.Controllers;

[Authorize(Roles = nameof(AccessLevel.Administrator))]
[Route("manufacturer")]
public class ManufacturerController(IManufacturerService manufacturerService) : Controller
{
    private readonly IManufacturerService _manufacturerService = manufacturerService;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        return View(manufacturers);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Manufacturer manufacturer)
    {
        if (!ModelState.IsValid)
            return View(manufacturer);

        try
        {
            await _manufacturerService.AddAsync(manufacturer);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(manufacturer);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
            return NotFound();

        return View(manufacturer);
    }

    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Manufacturer manufacturer)
    {
        if (id != manufacturer.ManufacturerId)
            return BadRequest("ID do fabricante não corresponde.");

        if (!ModelState.IsValid)
            return View(manufacturer);

        try
        {
            await _manufacturerService.UpdateAsync(manufacturer);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(manufacturer);
        }
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
            return NotFound();

        return View(manufacturer);
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _manufacturerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var manufacturer = await _manufacturerService.GetByIdAsync(id);
            return View(manufacturer);
        }
    }
}
