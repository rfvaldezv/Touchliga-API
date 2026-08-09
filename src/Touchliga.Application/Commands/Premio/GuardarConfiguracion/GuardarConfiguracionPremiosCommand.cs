using MediatR;

namespace Touchliga.Application.Commands.Premio.GuardarConfiguracion;

public sealed record ItemConfiguracionPremio(
    int Posicion,
    string TipoPremio,
    decimal Monto,
    string? Descripcion);

/// <summary>Guarda de un jalón toda la tabla de premios de un ámbito
/// (Jornada o Final) para una temporada — igual que Pronósticos, se
/// manda la tabla completa y se actualiza/crea lo que haga falta.</summary>
public sealed record GuardarConfiguracionPremiosCommand(
    long TemporadaId,
    string Ambito,
    List<ItemConfiguracionPremio> Premios
) : IRequest<Unit>;
