using MediatR;

namespace Touchliga.Application.Commands.Partido.MarcarDesempate;

/// <summary>Marca (o desmarca) un partido como el de la caja de
/// desempate de su jornada. Solo puede haber uno por jornada -- si
/// ya hay otro marcado, se desmarca automáticamente al marcar este.</summary>
public sealed record MarcarDesempateCommand(long PartidoId, bool EsDesempate) : IRequest<Unit>;
