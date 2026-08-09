using MediatR;

namespace Touchliga.Application.Commands.Jornada.Update;

public sealed record UpdateJornadaCommand(
    long Id,
    string Nombre,
    string Descripcion,
    int Numero,
    DateTime FechaCierre,
    bool Activo
)
    : IRequest<long>;
