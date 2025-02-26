using AutoFusion.Domain.Entities;

namespace AutoFusion.Web.Models;

public class DashboardViewModel
{
    public decimal TotalSalesAmount { get; set; } = 0;
    public int TotalSalesCount { get; set; } = 0;
    public int TotalCustomers { get; set; } = 0;
    public int TotalVehicles { get; set; } = 0;
    public int TotalDealerships { get; set; } = 0;
    public int TotalManufacturers { get; set; } = 0;
    public int TotalUsers { get; set; } = 0;

    public List<Sale> RecentSales { get; set; } = new();
    public List<Manufacturer> Manufacturers { get; set; } = new();
    public List<SalesByDealershipViewModel> SalesByDealership { get; set; } = new();
}

public class SalesByDealershipViewModel
{
    public string Dealership { get; set; } = string.Empty;
    public int TotalSales { get; set; } = 0;
}
