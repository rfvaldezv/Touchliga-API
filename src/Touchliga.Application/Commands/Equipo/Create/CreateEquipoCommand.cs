using MediatR;

namespace Touchliga.Application.Commands.Equipo.Create;

/// <summary>
/// Create Equipo.
/// </summary>
public sealed record CreateEquipoCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    string? EscudoUrl,
    string? Apodo,
    bool Activo
)
    : IRequest<long>;
