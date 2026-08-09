using MediatR;

namespace Touchliga.Application.Commands.Equipo.Update;

/// <summary>
/// Update Equipo.
/// </summary>
public sealed record UpdateEquipoCommand(
    long Id,
    string Nombre,
    string Descripcion,
    string? EscudoUrl,
    string? Apodo,
    bool Activo
)
    : IRequest<long>;
