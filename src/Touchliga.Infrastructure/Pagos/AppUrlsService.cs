using Microsoft.AspNetCore.Http;

using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Infrastructure.Pagos;

public sealed class AppUrlsService : IAppUrlsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppUrlsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string BaseUrlPublica
    {
        get
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request is null) return string.Empty;

            return $"{request.Scheme}://{request.Host}";
        }
    }
}
