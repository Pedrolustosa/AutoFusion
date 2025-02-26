using Microsoft.AspNetCore.Mvc;
using AutoFusion.Application.DTOs;
using AutoFusion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AutoFusion.Web.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = false)]
[Route("account")]
public class AccountController(IAccountService accountService) : Controller
{
    private readonly IAccountService _accountService = accountService;

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
    {
        if (!ModelState.IsValid)
            return View(loginRequest);

        try
        {
            var token = await _accountService.LoginAsync(loginRequest);
            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(loginRequest);
        }
    }

    [HttpGet("register")]
    public IActionResult Register() => View();

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
    {
        if (!ModelState.IsValid)
            return View(registerRequest);

        try
        {
            await _accountService.RegisterAsync(registerRequest);
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(registerRequest);
        }
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
