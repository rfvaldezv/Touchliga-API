using MediatR;

namespace Touchliga.Application.Commands.Cancha.Delete;

/// <summary>
/// Elimina un Cancha.
/// </summary>
public sealed record DeleteCanchaCommand(
    long Id)
    : IRequest<Unit>;
