using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoFusion.Infrastructure.Repositories;

public class ManufacturerRepository(ApplicationDbContext context) : IManufacturerRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Manufacturers.AnyAsync(m => m.Name == name);
    }

    public async Task AddAsync(Manufacturer manufacturer)
    {
        await _context.Manufacturers.AddAsync(manufacturer);
        await _context.SaveChangesAsync();
    }

    public async Task<Manufacturer> GetByIdAsync(int id)
    {
        return await _context.Manufacturers.AsNoTracking().FirstOrDefaultAsync(m => m.ManufacturerId == id);
    }

    public async Task<IEnumerable<Manufacturer>> GetAllAsync()
    {
        return await _context.Manufacturers.Where(m => !m.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<Manufacturer>> FindAsync(Expression<Func<Manufacturer, bool>> predicate)
    {
        return await _context.Manufacturers.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(Manufacturer entity)
    {
        _context.Manufacturers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id);
        if (manufacturer != null)
        {
            manufacturer.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}