using MediatR;

namespace Touchliga.Application.Commands.Estado.Delete;

/// <summary>
/// Elimina un Estado.
/// </summary>
public sealed record DeleteEstadoCommand(
    long Id)
    : IRequest<Unit>;
