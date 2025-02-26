using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AutoFusion.Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres.")]
    public string CPF { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Formato de telefone inválido.")]
    public string Phone { get; set; }
}
