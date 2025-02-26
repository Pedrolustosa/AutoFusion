using System.Linq.Expressions;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoFusion.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(Customer customer)
    {
        var existingCustomer = await FindByCpfAsync(customer.CPF);
        if (existingCustomer != null)
            throw new ArgumentException("Já existe um cliente cadastrado com este CPF.");

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer?> FindByCpfAsync(string cpf) // Implementação do método
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CPF == cpf);
    }

    public async Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate)
    {
        return await _context.Customers.Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
