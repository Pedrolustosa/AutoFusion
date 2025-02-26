using AutoFusion.Domain.Entities;

namespace AutoFusion.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(string id);
}
