using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Application.DTOs;

public class CreateManufacturerDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O país de origem é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
    public string CountryOfOrigin { get; set; }

    [Required(ErrorMessage = "O ano de fundação é obrigatório.")]
    [Range(1800, int.MaxValue, ErrorMessage = "O ano de fundação deve ser válido e no passado.")]
    public int FoundationYear { get; set; }

    [Required(ErrorMessage = "O website é obrigatório.")]
    [Url(ErrorMessage = "O URL do website deve ser válido.")]
    public string Website { get; set; }
}
