namespace Touchliga.Application.Authentication.DTOs;

public sealed class LoginRequest
{
    public string Correo { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
