using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pronostico.GetMiosPorJornada;

public sealed record GetMisPronosticosPorJornadaQuery(long JornadaId)
    : IRequest<IReadOnlyList<PronosticoDto>>;
