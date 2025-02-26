using System.Text;
using System.Security.Claims;
using AutoFusion.Domain.Entities;
using AutoFusion.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoFusion.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AutoFusion.Application.Services;

public class AccountService(UserManager<ApplicationUser> userManager,
                      SignInManager<ApplicationUser> signInManager,
                      IConfiguration configuration) : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username)
                   ?? throw new Exception("Invalid credentials.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        if (!result.Succeeded)
            throw new Exception("Invalid credentials.");

        // Sign in the user to issue a cookie
        await _signInManager.SignInAsync(user, isPersistent: false);

        // (Optional) Generate a JWT token for API use if needed
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.UserName),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.NameIdentifier, user.Id)
    };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task RegisterAsync(RegisterRequestDto registerRequest)
    {
        var userExists = await _userManager.FindByNameAsync(registerRequest.Username);
        if (userExists != null)
            throw new Exception("User already exists.");

        ApplicationUser newUser = new()
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email,
            AccessLevel = Enum.TryParse<AccessLevel>(registerRequest.AccessLevel, true, out var level)
                          ? level : AccessLevel.Salesperson
        };

        var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
