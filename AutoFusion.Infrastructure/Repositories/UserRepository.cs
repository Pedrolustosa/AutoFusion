using AutoFusion.Domain.Entities;
using AutoFusion.Domain.Interfaces;
using AutoFusion.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace AutoFusion.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await Task.FromResult(_userManager.Users.ToList());
    }

    public async Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return await Task.FromResult(_userManager.Users.Where(predicate).ToList());
    }

    public async Task AddAsync(ApplicationUser entity)
    {
        var result = await _userManager.CreateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task UpdateAsync(ApplicationUser entity)
    {
        var result = await _userManager.UpdateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task DeleteAsync(string id)
    {
        var user = await GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task<ApplicationUser?> FindByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return false;
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
