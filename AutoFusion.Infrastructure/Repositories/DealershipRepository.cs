using System.Linq.Expressions;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AutoFusion.Infrastructure.Repositories;

public class DealershipRepository(ApplicationDbContext context) : IDealershipRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(Dealership dealership)
    {
        var existingDealership = await FindByNameAsync(dealership.Name);
        if (existingDealership != null)
            throw new ArgumentException("Já existe uma concessionária com este nome.");

        await _context.Dealerships.AddAsync(dealership);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Dealership>> GetAllAsync()
    {
        return await _context.Dealerships.Where(d => !d.IsDeleted).ToListAsync();
    }

    public async Task<Dealership?> GetByIdAsync(int id)
    {
        return await _context.Dealerships.FindAsync(id);
    }

    public async Task<Dealership?> FindByNameAsync(string name)
    {
        return await _context.Dealerships.FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task<IEnumerable<Dealership>> FindAsync(Expression<Func<Dealership, bool>> predicate)
    {
        return await _context.Dealerships.Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(Dealership dealership)
    {
        _context.Dealerships.Update(dealership);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dealership = await _context.Dealerships.FindAsync(id);
        if (dealership != null)
        {
            dealership.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
