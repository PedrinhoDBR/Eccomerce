using Ecommerce.Contracts.Requests;
using Ecommerce.Contracts.Responses;
using Ecommerce.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly AdminAuthService _authService;

    public AuthController(AdminAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        if (!_authService.ValidateCredentials(request.Username, request.Password))
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new LoginResponse(_authService.CreateToken(request.Username), request.Username));
    }
}
