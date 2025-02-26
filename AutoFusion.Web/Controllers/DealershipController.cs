using Microsoft.AspNetCore.Mvc;
using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AutoFusion.Web.Controllers;

[Authorize(Roles = nameof(AccessLevel.Administrator))]
[Route("dealership")]
public class DealershipController(IDealershipService dealershipService) : Controller
{
    private readonly IDealershipService _dealershipService = dealershipService;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var dealerships = await _dealershipService.GetAllAsync();
        return View(dealerships);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Dealership dealership)
    {
        if (!ModelState.IsValid)
            return View(dealership);

        try
        {
            await _dealershipService.AddAsync(dealership);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dealership);
        }
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var dealership = await _dealershipService.GetByIdAsync(id);
        if (dealership == null)
            return NotFound();

        return View(dealership);
    }

    [HttpPost("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Dealership dealership)
    {
        if (id != dealership.DealershipId)
            return BadRequest("ID da concessionária não corresponde.");

        if (!ModelState.IsValid)
            return View(dealership);

        try
        {
            await _dealershipService.UpdateAsync(dealership);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dealership);
        }
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dealership = await _dealershipService.GetByIdAsync(id);
        if (dealership == null)
            return NotFound();

        return View(dealership);
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _dealershipService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var dealership = await _dealershipService.GetByIdAsync(id);
            return View(dealership);
        }
    }
}
