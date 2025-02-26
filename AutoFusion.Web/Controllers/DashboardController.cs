using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoFusion.Application.Interfaces;
using AutoFusion.Web.Models;

namespace AutoFusion.Web.Controllers;

[Authorize]
public class DashboardController(
    ISaleService saleService,
    ICustomerService customerService,
    IVehicleService vehicleService,
    IDealershipService dealershipService,
    IManufacturerService manufacturerService,
    IUserService userService) : Controller
{
    private readonly ISaleService _saleService = saleService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IVehicleService _vehicleService = vehicleService;
    private readonly IDealershipService _dealershipService = dealershipService;
    private readonly IManufacturerService _manufacturerService = manufacturerService;
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var sales = await _saleService.GetAllAsync();
        var customers = await _customerService.GetAllAsync();
        var vehicles = await _vehicleService.GetAllAsync();
        var dealerships = await _dealershipService.GetAllAsync();
        var manufacturers = await _manufacturerService.GetAllAsync();
        var users = await _userService.GetAllAsync();

        var salesByDealership = sales
            .Where(s => s.Dealership != null)
            .GroupBy(s => s.Dealership!.Name)
            .Select(g => new SalesByDealershipViewModel
            {
                Dealership = g.Key,
                TotalSales = g.Count()
            })
            .ToList();

        var dashboardData = new DashboardViewModel
        {
            TotalSalesAmount = sales.Sum(s => s.SalePrice),
            TotalSalesCount = sales.Count(),
            TotalCustomers = customers.Count(),
            TotalVehicles = vehicles.Count(),
            TotalDealerships = dealerships.Count(),
            TotalManufacturers = manufacturers.Count(),
            TotalUsers = users.Count(),
            RecentSales = sales.OrderByDescending(s => s.SaleDate).Take(5).ToList(),
            Manufacturers = manufacturers.ToList(),
            SalesByDealership = salesByDealership // 🔹 Enviando dados para o gráfico
        };

        return View(dashboardData);
    }
}
