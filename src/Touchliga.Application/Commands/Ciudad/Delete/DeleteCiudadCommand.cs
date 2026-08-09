using MediatR;

namespace Touchliga.Application.Commands.Ciudad.Delete;

/// <summary>
/// Elimina un Ciudad.
/// </summary>
public sealed record DeleteCiudadCommand(
    long Id)
    : IRequest<Unit>;
