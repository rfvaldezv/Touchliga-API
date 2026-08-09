using MediatR;

namespace Touchliga.Application.Commands.Jornada.Create;

public sealed record CreateJornadaCommand(
    long TemporadaId,
    string Codigo,
    string Nombre,
    string Descripcion,
    int Numero,
    DateTime FechaCierre,
    bool Activo
)
    : IRequest<long>;
