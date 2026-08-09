using MediatR;

namespace Touchliga.Application.Commands.Equipo.Delete;

/// <summary>
/// Elimina un Equipo.
/// </summary>
public sealed record DeleteEquipoCommand(
    long Id)
    : IRequest<Unit>;
