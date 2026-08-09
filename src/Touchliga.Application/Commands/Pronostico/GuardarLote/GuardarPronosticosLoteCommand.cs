using MediatR;

namespace Touchliga.Application.Commands.Pronostico.GuardarLote;

public sealed record PronosticoLoteItem(
    long PartidoId,
    long EquipoGanadorId,
    int? PuntosTotalesPredichos,
    int? DiferenciaPuntosPredicha);

/// <summary>
/// Guarda todos los pronósticos de una jornada en un solo paso (en
/// vez de uno por partido). Al terminar, si el usuario ya cubrió
/// TODOS los partidos de la jornada, se le manda un correo de
/// confirmación — por diseño, solo se evalúa una vez por lote, así
/// que nunca manda varios correos por una sola acción de "Guardar".
/// </summary>
public sealed record GuardarPronosticosLoteCommand(
    long JornadaId,
    List<PronosticoLoteItem> Pronosticos
)
    : IRequest<bool>;
