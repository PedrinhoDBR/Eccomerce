using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.Infrastructure.Security;

public sealed class AdminAuthorizationFilter : IAuthorizationFilter
{
    private readonly AdminAuthService _authService;

    public AdminAuthorizationFilter(AdminAuthService authService)
    {
        _authService = authService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers.Authorization.ToString();

        if (!_authService.ValidateToken(authorization))
        {
            context.Result = new UnauthorizedObjectResult("Admin login is required.");
        }
    }
}
