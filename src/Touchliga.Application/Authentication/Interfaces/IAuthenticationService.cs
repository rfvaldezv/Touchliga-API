using Touchliga.Application.Authentication.DTOs;

namespace Touchliga.Application.Authentication.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(
        string correo,
        string password,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}
