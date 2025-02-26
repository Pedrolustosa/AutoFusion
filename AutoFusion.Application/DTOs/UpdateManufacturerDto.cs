using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Application.DTOs;

public class UpdateManufacturerDto
{
    [Required]
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "Manufacturer name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Country of origin is required.")]
    [StringLength(50, ErrorMessage = "Country of origin must be at most 50 characters.")]
    public string CountryOfOrigin { get; set; }

    [Required(ErrorMessage = "Foundation year is required.")]
    [Range(1800, int.MaxValue, ErrorMessage = "Foundation year must be valid and in the past.")]
    public int FoundationYear { get; set; }

    [Required(ErrorMessage = "Website URL is required.")]
    [Url(ErrorMessage = "The website URL must be valid.")]
    public string Website { get; set; }
}
