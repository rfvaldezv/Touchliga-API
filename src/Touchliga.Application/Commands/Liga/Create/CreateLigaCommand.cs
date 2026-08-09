using MediatR;

namespace Touchliga.Application.Commands.Liga.Create;

/// <summary>
/// Create Liga.
/// </summary>
public sealed record CreateLigaCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
