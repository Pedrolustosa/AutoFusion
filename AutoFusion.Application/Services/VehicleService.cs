using System.Linq.Expressions;
using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class VehicleService(IVehicleRepository vehicleRepository, IManufacturerRepository manufacturerRepository) : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IManufacturerRepository _manufacturerRepository = manufacturerRepository;

    public async Task AddAsync(Vehicle vehicle)
    {
        if (string.IsNullOrWhiteSpace(vehicle.Model) || vehicle.Model.Length > 100)
            throw new ArgumentException("O nome do modelo deve ter no máximo 100 caracteres.");

        if (vehicle.ManufacturingYear > DateTime.Now.Year)
            throw new ArgumentException("O ano de fabricação não pode estar no futuro.");

        if (vehicle.Price <= 0)
            throw new ArgumentException("O preço deve ser um valor positivo.");

        if (vehicle.ManufacturerId <= 0)
            throw new ArgumentException("O fabricante deve ser selecionado.");

        await _vehicleRepository.AddAsync(vehicle);
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync() => await _vehicleRepository.GetAllAsync();

    public async Task<Vehicle?> GetByIdAsync(int id) => await _vehicleRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Vehicle>> FindAsync(Expression<Func<Vehicle, bool>> predicate)
    {
        return await _vehicleRepository.FindAsync(predicate);
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        var existingVehicle = await _vehicleRepository.GetByIdAsync(vehicle.VehicleId);
        if (existingVehicle == null)
            throw new ArgumentException("Veículo não encontrado.");

        existingVehicle.Model = vehicle.Model;
        existingVehicle.ManufacturingYear = vehicle.ManufacturingYear;
        existingVehicle.Price = vehicle.Price;
        existingVehicle.ManufacturerId = vehicle.ManufacturerId;
        existingVehicle.VehicleType = vehicle.VehicleType;
        existingVehicle.Description = vehicle.Description;

        await _vehicleRepository.UpdateAsync(existingVehicle);
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle == null)
            throw new ArgumentException("Veículo não encontrado.");

        vehicle.IsDeleted = true;
        await _vehicleRepository.UpdateAsync(vehicle);
    }
}
