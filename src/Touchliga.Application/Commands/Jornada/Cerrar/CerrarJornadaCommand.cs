using MediatR;

namespace Touchliga.Application.Commands.Jornada.Cerrar;

/// <summary>
/// Cierra la jornada y calcula los puntos de todos los pronósticos
/// de sus partidos (requiere que cada partido ya tenga resultado
/// capturado; los partidos sin resultado se ignoran en el cálculo).
/// </summary>
public sealed record CerrarJornadaCommand(long Id) : IRequest<Unit>;
