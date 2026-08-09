using MediatR;

namespace Touchliga.Application.Commands.Temporada.Update;

public sealed record UpdateTemporadaCommand(
    long Id,
    string Nombre,
    string Descripcion,
    DateTime FechaInicio,
    DateTime FechaFin,
    decimal Cuota,
    bool Activo
)
    : IRequest<long>;
