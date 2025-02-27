using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoFusion.Application.Interfaces;
using AutoFusion.Application.DTOs;
using AutoFusion.Domain.Entities;

namespace AutoFusion.Web.Controllers;

[Authorize(Roles = nameof(AccessLevel.Administrator))]
[Route("reporting")]
public class ReportingController(IReportingService reportingService) : Controller
{
    private readonly IReportingService _reportingService = reportingService;

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DownloadReportPdf(ReportFilterDto filter)
    {
        if (filter == null || string.IsNullOrEmpty(filter.ReportType))
        {
            return BadRequest("Tipo de relatório inválido.");
        }

        var pdfBytes = await _reportingService.GenerateReportPdfAsync(filter);

        if (pdfBytes == null || pdfBytes.Length == 0)
        {
            return BadRequest("Erro ao gerar o relatório.");
        }

        return File(pdfBytes, "application/pdf", $"report_{filter.ReportType}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf");
    }
}
