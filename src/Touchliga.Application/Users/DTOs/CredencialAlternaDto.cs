namespace Touchliga.Application.Users.DTOs;

/// <summary>Nunca incluye la contraseña -- ni siquiera el hash.</summary>
public sealed class CredencialAlternaDto
{
    public string Correo { get; set; } = string.Empty;
}
