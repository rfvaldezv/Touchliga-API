using MediatR;

namespace Touchliga.Application.Commands.Pais.Delete;

/// <summary>
/// Elimina un Pais.
/// </summary>
public sealed record DeletePaisCommand(
    long Id)
    : IRequest<Unit>;
