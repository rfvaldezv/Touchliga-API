using MediatR;

namespace Touchliga.Application.Commands.Pais.Create;

/// <summary>
/// Create Pais.
/// </summary>
public sealed record CreatePaisCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
