using AutoFusion.Domain.Entities;

namespace AutoFusion.Domain.Interfaces;

public interface IManufacturerRepository : IGenericRepository<Manufacturer, int>
{
    Task<bool> ExistsByNameAsync(string name);
}
