using AutoFusion.Domain.Entities;

namespace AutoFusion.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser, string>
{
    Task<ApplicationUser?> FindByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
}
