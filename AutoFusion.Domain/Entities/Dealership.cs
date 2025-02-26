using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Domain.Entities;

public class Dealership
{
    [Key]
    public int DealershipId { get; set; }

    [Required(ErrorMessage = "O nome da concessionária é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da concessionária deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
    public string City { get; set; }

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres.")]
    public string State { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-\d{4}$", ErrorMessage = "O telefone deve estar no formato (XX) XXXXX-XXXX.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail deve estar em um formato válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A capacidade máxima de veículos é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima deve ser um número positivo.")]
    public int MaxVehicleCapacity { get; set; }

    public bool IsDeleted { get; set; } = false;
}
