using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Application.DTOs;

public class ReportFilterDto
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    public string? ReportType { get; set; }
    
    public string[]? Categories { get; set; }
    
    public bool IncludeInactive { get; set; }
    
    public int? DealershipId { get; set; }
    
    public int? ManufacturerId { get; set; }
}
