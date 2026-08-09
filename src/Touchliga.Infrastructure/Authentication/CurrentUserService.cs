using System.Security.Claims;
using Touchliga.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Touchliga.Infrastructure.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public long UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User?.FindFirstValue("sub");

            return long.TryParse(value, out var id)
                ? id
                : 0;
        }
    }

    public string Nombre =>
        User?.FindFirstValue(ClaimTypes.Name)
        ?? User?.FindFirstValue("name")
        ?? string.Empty;

    public string Correo =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? User?.FindFirstValue("email")
        ?? string.Empty;
}
