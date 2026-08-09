using MediatR;

namespace Touchliga.Application.Commands.Temporada.Delete;

/// <summary>
/// Elimina un Temporada.
/// </summary>
public sealed record DeleteTemporadaCommand(
    long Id)
    : IRequest<Unit>;
