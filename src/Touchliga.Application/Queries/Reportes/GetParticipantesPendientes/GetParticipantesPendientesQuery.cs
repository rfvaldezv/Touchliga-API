using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Reportes.GetParticipantesPendientes;

public sealed record GetParticipantesPendientesQuery(long JornadaId) : IRequest<List<ParticipantePendienteDto>>;
