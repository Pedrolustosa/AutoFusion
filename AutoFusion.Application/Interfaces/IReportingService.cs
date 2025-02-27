using AutoFusion.Application.DTOs;

namespace AutoFusion.Application.Interfaces;

public interface IReportingService
{
    Task<byte[]> GenerateReportPdfAsync(ReportFilterDto filter);
}
