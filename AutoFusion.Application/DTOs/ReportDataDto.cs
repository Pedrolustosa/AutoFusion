namespace AutoFusion.Application.DTOs;

public class ReportDataDto
{
    public string ReportTitle { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public ReportFilterDto Filter { get; set; } = null!;
    public List<ReportItemDto> Items { get; set; } = new();
    public ReportSummaryDto Summary { get; set; } = new();
}

public class ReportItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? DealershipName { get; set; }
    public string? ManufacturerName { get; set; }
    public Dictionary<string, object> AdditionalData { get; set; } = new();
}

public class ReportSummaryDto
{
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
    public Dictionary<string, decimal> CategoryTotals { get; set; } = new();
    public Dictionary<string, object> AdditionalMetrics { get; set; } = new();
}
