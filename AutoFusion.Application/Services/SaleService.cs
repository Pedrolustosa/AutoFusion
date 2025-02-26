using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using System.Linq.Expressions;

namespace AutoFusion.Application.Services;

public class SaleService(
    ISaleRepository saleRepository,
    IVehicleRepository vehicleRepository,
    ICustomerRepository customerRepository,
    IDealershipRepository dealershipRepository) : ISaleService
{
    private readonly ISaleRepository _saleRepository = saleRepository;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IDealershipRepository _dealershipRepository = dealershipRepository;

    public async Task AddAsync(Sale sale)
    {
        if (string.IsNullOrWhiteSpace(sale.Customer.CPF) || !IsValidCPF(sale.Customer.CPF))
            throw new ArgumentException("CPF inválido.");

        var existingCustomer = await _customerRepository.FindByCpfAsync(sale.Customer.CPF);
        if (existingCustomer == null)
        {
            existingCustomer = new Customer
            {
                Name = sale.Customer.Name,
                CPF = sale.Customer.CPF,
                Phone = sale.Customer.Phone
            };

            await _customerRepository.AddAsync(existingCustomer);
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(sale.VehicleId);
        if (vehicle == null)
            throw new ArgumentException("Veículo não encontrado.");

        if (sale.SalePrice > vehicle.Price)
            throw new ArgumentException("O preço de venda não pode ser superior ao preço do veículo.");

        var dealership = await _dealershipRepository.GetByIdAsync(sale.DealershipId);
        if (dealership == null)
            throw new ArgumentException("Concessionária não encontrada.");

        sale.SaleProtocol = GenerateSaleProtocol();
        sale.Customer = existingCustomer;
        sale.Vehicle = vehicle;
        sale.Dealership = dealership;

        await _saleRepository.AddAsync(sale);
    }

    private static bool IsValidCPF(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();
        return cpf.Length == 11 && cpf.All(char.IsDigit);
    }

    public async Task<IEnumerable<Sale>> GetAllAsync() => await _saleRepository.GetAllAsync();

    public async Task<Sale?> GetByIdAsync(int id) => await _saleRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Sale>> FindAsync(Expression<Func<Sale, bool>> predicate)
    {
        return await _saleRepository.FindAsync(predicate);
    }

    public async Task UpdateAsync(Sale sale) => await _saleRepository.UpdateAsync(sale);

    public async Task DeleteAsync(int id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
            throw new ArgumentException("Venda não encontrada.");

        sale.IsDeleted = true;
        await _saleRepository.UpdateAsync(sale);
    }

    private static string GenerateSaleProtocol()
    {
        return $"SALE-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
