using Microsoft.AspNetCore.Mvc;
using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AutoFusion.Web.Controllers;

[Authorize(Roles = nameof(AccessLevel.Administrator))]
[Route("sale")]
public class SaleController(ISaleService saleService, IVehicleService vehicleService, IDealershipService dealershipService) : Controller
{
    private readonly ISaleService _saleService = saleService;
    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IDealershipService _dealershipService = dealershipService;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var sales = await _saleService.GetAllAsync();
        return View(sales);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Vehicles = await _vehicleService.GetAllAsync();
        ViewBag.Dealerships = await _dealershipService.GetAllAsync();
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Sale sale)
    {
        sale.Customer ??= new Customer();
        ModelState.Remove(nameof(sale.Vehicle));
        ModelState.Remove(nameof(sale.Dealership));
        ModelState.Remove(nameof(sale.SaleProtocol));

        if (string.IsNullOrWhiteSpace(sale.Customer.CPF))
        {
            ModelState.AddModelError("Customer.CPF", "O CPF é obrigatório.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Vehicles = await _vehicleService.GetAllAsync();
            ViewBag.Dealerships = await _dealershipService.GetAllAsync();
            return View(sale);
        }

        try
        {
            await _saleService.AddAsync(sale);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Vehicles = await _vehicleService.GetAllAsync();
            ViewBag.Dealerships = await _dealershipService.GetAllAsync();
            return View(sale);
        }
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        if (sale == null)
            return NotFound();

        return View(sale);
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        if (sale == null)
            return NotFound();

        return View(sale);
    }

    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _saleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var sale = await _saleService.GetByIdAsync(id);
            return View(sale);
        }
    }
}
