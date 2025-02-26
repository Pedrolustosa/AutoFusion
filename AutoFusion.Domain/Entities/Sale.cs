using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Domain.Entities;

public class Sale
{
    [Key]
    public int SaleId { get; set; }

    [Required(ErrorMessage = "A concessionária é obrigatória.")]
    public int DealershipId { get; set; }

    [Required(ErrorMessage = "O veículo é obrigatório.")]
    public int VehicleId { get; set; }

    [Required(ErrorMessage = "O cliente é obrigatório.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "A data da venda é obrigatória.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Sale), nameof(ValidateSaleDate))]
    public DateTime SaleDate { get; set; }

    [Required(ErrorMessage = "O preço da venda é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço da venda deve ser um valor positivo.")]
    public decimal SalePrice { get; set; }

    [Required(ErrorMessage = "O número do protocolo da venda é obrigatório.")]
    public string SaleProtocol { get; set; }

    public bool IsDeleted { get; set; } = false;

    public Vehicle Vehicle { get; set; }
    public Dealership Dealership { get; set; }
    public Customer Customer { get; set; }

    public static ValidationResult ValidateSaleDate(DateTime saleDate, ValidationContext context)
    {
        return saleDate > DateTime.Today
            ? new ValidationResult("A data da venda não pode ser futura.")
            : ValidationResult.Success;
    }
}
