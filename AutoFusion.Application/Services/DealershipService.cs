using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class DealershipService(IDealershipRepository dealershipRepository) : IDealershipService
{
    private readonly IDealershipRepository _dealershipRepository = dealershipRepository;

    public async Task AddAsync(Dealership dealership)
    {
        if (string.IsNullOrWhiteSpace(dealership.Name) || dealership.Name.Length > 100)
            throw new ArgumentException("O nome da concessionária deve ter no máximo 100 caracteres.");

        await _dealershipRepository.AddAsync(dealership);
    }

    public async Task<IEnumerable<Dealership>> GetAllAsync() => await _dealershipRepository.GetAllAsync();

    public async Task<Dealership?> GetByIdAsync(int id) => await _dealershipRepository.GetByIdAsync(id);

    public async Task UpdateAsync(Dealership dealership) => await _dealershipRepository.UpdateAsync(dealership);

    public async Task DeleteAsync(int id)
    {
        var dealership = await _dealershipRepository.GetByIdAsync(id);
        if (dealership == null)
            throw new ArgumentException("Concessionária não encontrada.");

        dealership.IsDeleted = true;
        await _dealershipRepository.UpdateAsync(dealership);
    }
}
