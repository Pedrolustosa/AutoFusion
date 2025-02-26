using AutoFusion.Application.DTOs;

namespace AutoFusion.Application.Interfaces;

public interface IAccountService
{
    Task<string> LoginAsync(LoginRequestDto loginRequest);
    Task RegisterAsync(RegisterRequestDto registerRequest);
    Task SignOutAsync();
}
