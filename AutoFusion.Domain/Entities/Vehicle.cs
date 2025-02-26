using System.ComponentModel.DataAnnotations;

namespace AutoFusion.Domain.Entities;

public class Vehicle
{
    public int VehicleId { get; set; }

    [Required(ErrorMessage = "O modelo do veículo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
    public string Model { get; set; }

    [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
    public int ManufacturingYear { get; set; }

    [Required(ErrorMessage = "O preço do veículo é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O fabricante é obrigatório.")]
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "O tipo do veículo é obrigatório.")]
    public VehicleType VehicleType { get; set; }

    [Required(ErrorMessage = "A descrição do veículo é obrigatório.")]
    [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
    public string Description { get; set; }

    public bool IsDeleted { get; set; } = false;

    public Manufacturer Manufacturer { get; set; }
}

public enum VehicleType
{
    [Display(Name = "Carro")]
    Car,

    [Display(Name = "Moto")]
    Motorcycle,

    [Display(Name = "Caminhão")]
    Truck
}
