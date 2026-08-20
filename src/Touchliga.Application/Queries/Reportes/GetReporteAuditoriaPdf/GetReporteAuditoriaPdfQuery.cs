using MediatR;

namespace Touchliga.Application.Queries.Reportes.GetReporteAuditoriaPdf;

/// <summary>Genera el PDF de auditoría de una jornada: tabla con
/// cada participante y sus pronósticos de cada partido, pensado para
/// compartirse en el grupo de WhatsApp.</summary>
public sealed record GetReporteAuditoriaPdfQuery(long JornadaId) : IRequest<byte[]>;
