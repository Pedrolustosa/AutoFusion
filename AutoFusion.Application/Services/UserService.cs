using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
