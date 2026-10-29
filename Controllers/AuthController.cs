using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApplication5.Auth;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/auth")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var result = await authService.RegisterAsync(dto);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : ErrorResult(result);
    }

    [EnableRateLimiting(LoginRateLimit.PolicyName)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await authService.LoginAsync(dto);
        return result.IsSuccess ? Ok(result.Value) : ErrorResult(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDTO>> Me()
    {
        var user = await authService.GetCurrentUserAsync(User.GetUserId());
        return user is null ? NotFound() : Ok(user);
    }
}
