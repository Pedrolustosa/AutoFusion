using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoFusion.Infrastructure.Repositories;

public class VehicleRepository(ApplicationDbContext context) : IVehicleRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(Vehicle entity)
    {
        await _context.Vehicles.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.Manufacturer)
            .Where(v => !v.IsDeleted)
            .ToListAsync();
    }


    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.Manufacturer)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.VehicleId == id && !v.IsDeleted);
    }

    public async Task<IEnumerable<Vehicle>> FindAsync(Expression<Func<Vehicle, bool>> predicate)
    {
        return await _context.Vehicles.Include(v => v.Manufacturer)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task UpdateAsync(Vehicle entity)
    {
        _context.Vehicles.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle != null)
        {
            vehicle.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
