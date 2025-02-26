using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoFusion.Infrastructure.Repositories;

public class SaleRepository(ApplicationDbContext context) : ISaleRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Vehicle)
            .Include(s => s.Dealership)
            .Include(s => s.Customer)
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Vehicle)
            .Include(s => s.Dealership)
            .Include(s => s.Customer)
            .Where(s => !s.IsDeleted && s.SaleId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Sale>> FindAsync(Expression<Func<Sale, bool>> predicate)
    {
        return await _context.Sales
            .Include(s => s.Vehicle)
            .Include(s => s.Dealership)
            .Include(s => s.Customer)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale != null)
        {
            sale.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
