using MediatR;

namespace Touchliga.Application.Commands.Liga.Delete;

/// <summary>
/// Elimina un Liga.
/// </summary>
public sealed record DeleteLigaCommand(
    long Id)
    : IRequest<Unit>;
