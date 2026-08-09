namespace Touchliga.Application.Authentication.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(
        long usuarioId,
        string nombre,
        string correo,
        IEnumerable<string> roles);

    string GenerateRefreshToken();

    DateTime GetAccessTokenExpiration();
}
