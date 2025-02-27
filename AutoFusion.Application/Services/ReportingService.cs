using AutoFusion.Application.DTOs;
using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace AutoFusion.Application.Services;

public class ReportingService(
    ISaleRepository saleRepository,
    IVehicleRepository vehicleRepository,
    IDealershipRepository dealershipRepository,
    IManufacturerRepository manufacturerRepository) : IReportingService
{
    private readonly ISaleRepository _saleRepository = saleRepository;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDealershipRepository _dealershipRepository = dealershipRepository;
    private readonly IManufacturerRepository _manufacturerRepository = manufacturerRepository;

    public async Task<ReportDataDto> GenerateReportDataAsync(ReportFilterDto filter)
    {
        var reportData = new ReportDataDto
        {
            ReportTitle = $"{filter.ReportType} Report",
            GeneratedAt = DateTime.UtcNow,
            Filter = filter,
            Items = []
        };

        switch (filter.ReportType.ToLower())
        {
            case "vehicle":
                var vehicles = await _vehicleRepository.GetAllAsync();
                reportData.Items = vehicles.Where(v => !v.IsDeleted)
                                           .Select(v => new ReportItemDto
                                           {
                                               Id = v.VehicleId.ToString(),
                                               Name = v.Model,
                                               Amount = v.Price,
                                               Date = DateTime.UtcNow
                                           }).ToList();
                break;

            case "sale":
                var sales = await _saleRepository.GetAllAsync();
                reportData.Items = sales.Where(s => !s.IsDeleted)
                                        .Select(s => new ReportItemDto
                                        {
                                            Id = s.SaleId.ToString(),
                                            Name = s.SaleProtocol,
                                            Amount = s.SalePrice,
                                            Date = s.SaleDate
                                        }).ToList();
                break;

            case "dealership":
                var dealerships = await _dealershipRepository.GetAllAsync();
                reportData.Items = dealerships.Where(d => !d.IsDeleted)
                                              .Select(d => new ReportItemDto
                                              {
                                                  Id = d.DealershipId.ToString(),
                                                  Name = d.Name,
                                                  Amount = 0,
                                                  Date = DateTime.UtcNow
                                              }).ToList();
                break;

            case "manufacturer":
                var manufacturers = await _manufacturerRepository.GetAllAsync();
                reportData.Items = manufacturers.Where(m => !m.IsDeleted)
                                                .Select(m => new ReportItemDto
                                                {
                                                    Id = m.ManufacturerId.ToString(),
                                                    Name = m.Name,
                                                    Amount = 0,
                                                    Date = DateTime.UtcNow
                                                }).ToList();
                break;

            default:
                throw new ArgumentException("Invalid report type");
        }

        reportData.Summary.TotalItems = reportData.Items.Count;
        reportData.Summary.TotalAmount = reportData.Items.Sum(i => i.Amount);

        return reportData;
    }

    public async Task<byte[]> GenerateReportPdfAsync(ReportFilterDto filter)
    {
        var reportData = await GenerateReportDataAsync(filter);

        using var memoryStream = new MemoryStream();

        try
        {
            using var writer = new PdfWriter(memoryStream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            document.Add(new Paragraph(reportData.ReportTitle).SetFontSize(20));
            document.Add(new Paragraph($"Generated at: {reportData.GeneratedAt}").SetFontSize(10));

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddHeaderCell("Category");
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Amount");

            foreach (var item in reportData.Items)
            {
                table.AddCell(new Cell().Add(new Paragraph(item.Name)));
                table.AddCell(new Cell().Add(new Paragraph(item.Date.ToString("d"))));
                table.AddCell(new Cell().Add(new Paragraph(item.Amount.ToString("C"))));
            }

            document.Add(table);

            document.Add(new Paragraph("\nSummary").SetFontSize(16));
            document.Add(new Paragraph($"Total Items: {reportData.Summary.TotalItems}"));
            document.Add(new Paragraph($"Total Amount: {reportData.Summary.TotalAmount:C}"));

            document.Close();
            writer.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao gerar o PDF", ex);
        }

        return memoryStream.ToArray();
    }
}
