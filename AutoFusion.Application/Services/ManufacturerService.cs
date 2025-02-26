using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class ManufacturerService(IManufacturerRepository manufacturerRepository) : IManufacturerService
{
    private readonly IManufacturerRepository _manufacturerRepository = manufacturerRepository;

    public async Task<Manufacturer> GetByIdAsync(int id)
    {
        return await _manufacturerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Manufacturer>> GetAllAsync()
    {
        return await _manufacturerRepository.GetAllAsync();
    }

    public async Task AddAsync(Manufacturer entity)
    {
        await _manufacturerRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(Manufacturer entity)
    {
        await _manufacturerRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
        if (manufacturer == null)
            throw new ArgumentException("Fabricante não encontrado.");

        manufacturer.IsDeleted = true;
        await _manufacturerRepository.UpdateAsync(manufacturer);
    }
}
