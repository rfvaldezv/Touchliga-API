using MediatR;

namespace Touchliga.Application.Commands.Temporada.Create;

public sealed record CreateTemporadaCommand(
    long LigaId,
    string Codigo,
    string Nombre,
    string Descripcion,
    DateTime FechaInicio,
    DateTime FechaFin,
    decimal Cuota,
    bool Activo
)
    : IRequest<long>;
