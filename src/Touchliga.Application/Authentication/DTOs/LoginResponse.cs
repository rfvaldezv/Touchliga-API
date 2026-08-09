namespace Touchliga.Application.Authentication.DTOs;

public sealed class LoginResponse
{
    public long UsuarioId { get; init; }

    public string Nombre { get; init; } = string.Empty;

    public string Correo { get; init; } = string.Empty;

    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime Expira { get; init; }

    public List<string> Roles { get; init; } = new();
}
