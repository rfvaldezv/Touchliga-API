using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Estadisticas.GetEstadisticasParticipante;

public sealed record GetEstadisticasParticipanteQuery(long TemporadaId) : IRequest<EstadisticasParticipanteDto>;
