using MediatR;

namespace Touchliga.Application.Users.Commands.ActualizarPerfil;

/// <summary>
/// El propio usuario completa datos opcionales de su perfil
/// (fecha de nacimiento, equipo favorito, apodo) en cualquier
/// momento después de su primer ingreso. Siempre actúa sobre el
/// usuario autenticado — nunca recibe el id por parámetro.
/// </summary>
public sealed record ActualizarPerfilCommand(
    DateTime? FechaNacimiento,
    long? EquipoFavoritoId,
    string? Nickname,
    string? FotoUrl
)
    : IRequest<Unit>;
