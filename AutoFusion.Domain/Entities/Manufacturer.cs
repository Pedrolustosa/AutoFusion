using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Domain.Entities;

public class Manufacturer
{
    [Key]
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do fabricante deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O país de origem é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
    public string CountryOfOrigin { get; set; }

    [Required(ErrorMessage = "O ano de fundação é obrigatório.")]
    [Range(1800, 2100, ErrorMessage = "O ano de fundação deve estar no passado e ser um ano válido.")]
    public int FoundationYear { get; set; }

    [Url(ErrorMessage = "O website deve ser um URL válido.")]
    [StringLength(200, ErrorMessage = "O website deve ter no máximo 200 caracteres.")]
    public string Website { get; set; }

    public bool IsDeleted { get; set; } = false;
}
