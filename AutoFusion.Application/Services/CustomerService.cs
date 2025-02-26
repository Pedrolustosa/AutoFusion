using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Name) || customer.Name.Length > 100)
            throw new ArgumentException("O nome do cliente é obrigatório e deve ter no máximo 100 caracteres.");

        if (string.IsNullOrWhiteSpace(customer.CPF) || customer.CPF.Length != 11)
            throw new ArgumentException("O CPF é inválido.");

        var existingCustomer = await _customerRepository.FindByCpfAsync(customer.CPF);
        if (existingCustomer != null)
            throw new ArgumentException("Já existe um cliente com este CPF.");

        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Name) || customer.Name.Length > 100)
            throw new ArgumentException("O nome do cliente é obrigatório e deve ter no máximo 100 caracteres.");

        if (string.IsNullOrWhiteSpace(customer.CPF) || customer.CPF.Length != 11)
            throw new ArgumentException("O CPF é inválido.");

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}
