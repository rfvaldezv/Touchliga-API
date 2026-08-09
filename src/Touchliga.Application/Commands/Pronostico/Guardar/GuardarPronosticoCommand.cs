using MediatR;

namespace Touchliga.Application.Commands.Pronostico.Guardar;

/// <summary>
/// Crea o actualiza (upsert) el pronóstico del usuario autenticado
/// para un partido. El usuario se toma siempre de la sesión, nunca
/// del cliente, para que nadie pueda capturar pronósticos a nombre
/// de otro.
/// </summary>
public sealed record GuardarPronosticoCommand(
    long PartidoId,
    long EquipoGanadorId,
    int? PuntosTotalesPredichos,
    int? DiferenciaPuntosPredicha
)
    : IRequest<long>;
